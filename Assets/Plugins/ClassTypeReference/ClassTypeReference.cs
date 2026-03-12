using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TypeReference
{
    [Serializable]
    public sealed class ClassTypeReference : ISerializationCallbackReceiver
    {
        public string ClassRef => _classRef;
        
        public Type Type
        {
            get
            {
#if UNITY_EDITOR
                
                if (_type == null)
                {
                    if (string.IsNullOrEmpty(_classRef) == false)
                    {
                        TryGetTypeByGuid(_typeScriptFileGuid, out _type);
                    }
                }
#endif

            
                return _type; 
                
            }
            set
            {
                if (value != null && !value.IsClass)
                    throw new ArgumentException($"'{value.FullName}' is not a class type.", "value");

                _type = value;
                _classRef = GetClassRef(value);

#if UNITY_EDITOR
                _typeScriptFileGuid = GetTypeGuid(_type);
#endif
            }
        }

        [SerializeField] private string _classRef;

#if UNITY_EDITOR
        public string TypeScriptFileGuid => _typeScriptFileGuid;

        [SerializeField] private string _typeScriptFileGuid;
#endif
        
        private Type _type;

        public ClassTypeReference()
        {
        }

        public ClassTypeReference(string assemblyQualifiedClassName)
        {
            _type = !string.IsNullOrEmpty(assemblyQualifiedClassName)
                ? Type.GetType(assemblyQualifiedClassName)
                : null;

            if (_type == null) return;
            
            _classRef = GetClassRef(_type);
        }

        public ClassTypeReference(Type type)
        {
            _type = type;
            _classRef = GetClassRef(type);
        }
        
        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(_classRef))
            {
                _type = Type.GetType(_classRef);

#if UNITY_EDITOR
                
                if (_type == null)
                {
                    if (TryGetTypeByGuid(_typeScriptFileGuid, out _type) == false)
                    {
                        _classRef = GetClassRef(_type);
                        Debug.LogWarning($"'{_classRef}' was referenced but class type was not found.");
                    }
                }
                else if (_type != null && string.IsNullOrEmpty(_typeScriptFileGuid))
                {
                    _typeScriptFileGuid = GetTypeGuid(_type);
                }
#endif

            }
            else
            {
                _type = null;
            }
        }
        
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            
            if (string.IsNullOrEmpty(_typeScriptFileGuid))
            {
                _typeScriptFileGuid = GetTypeGuid(_type);
            }
            
#endif

        }

        public static implicit operator string(ClassTypeReference typeReference)
        {
            return typeReference._classRef;
        }

        public static implicit operator Type(ClassTypeReference typeReference)
        {
            return typeReference.Type;
        }

        public static implicit operator ClassTypeReference(Type type)
        {
            return new ClassTypeReference(type);
        }

        public override string ToString()
        {
            return Type != null ? Type.FullName : "(None)";
        }
        
        public static string GetClassRef(Type type)
        {
            return type != null
                ? type.FullName + ", " + type.Assembly.GetName().Name
                : "";
        }

#if UNITY_EDITOR

        public static string GetTypeGuid(Type componentType)
        {
            if (componentType == default)
            {
                return string.Empty;
            }
            
            try
            {
                string[] guids = AssetDatabase.FindAssets($"t:{nameof(MonoScript)}");

                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                    if (script.GetClass() == componentType) return guid;
                }

                return default;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        private bool TryGetTypeByGuid(string guid, out Type type)
        {
            type = default;
            if(string.IsNullOrEmpty(guid)) return false;
            
            try
            {
        
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                if (monoScript == null) return false;

                type = monoScript.GetClass();
                return true;
            }
            catch (Exception e)
            {
                type = default;
                return false;
            }
        }

#endif
    }
}