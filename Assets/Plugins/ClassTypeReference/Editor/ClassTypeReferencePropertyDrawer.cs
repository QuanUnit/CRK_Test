// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Collections.Generic;
using System.Reflection;
using ClassTypeReference.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace TypeReference.Editor
{
    /// <summary>
    /// Custom property drawer for <see cref="ClassTypeReference"/> properties.
    /// </summary>
    [CustomPropertyDrawer(typeof(ClassTypeReference))]
    [CustomPropertyDrawer(typeof(ClassTypeConstraintAttribute), true)]
    public sealed class ClassTypeReferencePropertyDrawer : PropertyDrawer
    {
        #region Type Filtering

        public static Func<ICollection<Type>> ExcludedTypeCollectionGetter { get; set; }

        private List<Type> GetFilteredTypes(ClassTypeConstraintAttribute filter)
        {
            var types = new List<Type>();

            var excludedTypes = (ExcludedTypeCollectionGetter != null ? ExcludedTypeCollectionGetter() : null);

            var assembly = Assembly.GetExecutingAssembly();
            FilterTypes(assembly, filter, excludedTypes, types);

            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
                FilterTypes(Assembly.Load(referencedAssembly), filter, excludedTypes, types);

            types.Sort((a, b) => a.FullName.CompareTo(b.FullName));

            return types;
        }

        private static void FilterTypes(Assembly assembly, ClassTypeConstraintAttribute filter,
            ICollection<Type> excludedTypes, List<Type> output)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsVisible || !type.IsClass)
                    continue;

                if (filter != null && !filter.IsConstraintSatisfied(type))
                    continue;

                if (excludedTypes != null && excludedTypes.Contains(type))
                    continue;

                output.Add(type);
            }
        }

        #endregion

        #region Type Utility

        private static Dictionary<string, Type> s_TypeMap = new Dictionary<string, Type>();

        private static Type ResolveType(string classRef) {
            Type type;
            if (!s_TypeMap.TryGetValue(classRef, out type)) {
                type = !string.IsNullOrEmpty(classRef) ? Type.GetType(classRef) : null;
                s_TypeMap[classRef] = type;
            }
            return type;
        }

        #endregion
        
        #region Control Drawing / Event Handling

        private readonly int s_ControlHint = typeof(ClassTypeReferencePropertyDrawer).GetHashCode();
        private SerializedProperty _drawingClassTypeNameProperty;
        private SerializedProperty _drawingClassTypeGuidProperty;
        
        private SerializedProperty _selectedClassTypeNameProperty;
        private SerializedProperty _selectedClassTypeGuidProperty;

        private bool _classTypeNamePropertyChanged;
        private string _classTypeNamePropertyValue;
        
        private void DrawTypeSelectionControlInternal(Rect position, GUIContent label, ClassTypeConstraintAttribute filter)
        {
            if (label != null && label != GUIContent.none)
                position = EditorGUI.PrefixLabel(position, label);

            int controlID = GUIUtility.GetControlID(s_ControlHint, FocusType.Keyboard, position);

            bool triggerDropDown = false;

            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                {
                    if (GUI.enabled && position.Contains(Event.current.mousePosition))
                    {
                        triggerDropDown = true;
                    }

                    break;
                }

                case EventType.KeyDown:
                {
                    if (GUI.enabled && GUIUtility.keyboardControl == controlID)
                    {
                        if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.Space)
                        {
                            triggerDropDown = true;
                        }
                    }

                    break;
                }

                case EventType.Repaint:
                {
                    string classTypeFullName = _drawingClassTypeNameProperty.stringValue;
                    string[] classRefParts = classTypeFullName.Split(',');

                    label.text = classRefParts[0].Split('.')[^1].Trim();
                    if (label.text == "")
                        label.text = "(None)";
                    else if (ResolveType(classTypeFullName) == null)
                        label.text = $"<Missing> {label.text}";

                    EditorStyles.popup.Draw(position, label, controlID);
                    break;
                }
            }

            if (triggerDropDown)
            {
                List<Type> filteredTypes = GetFilteredTypes(filter);
                DisplayDropDown(position, filteredTypes, filter.Grouping);
                
                _selectedClassTypeGuidProperty = _drawingClassTypeGuidProperty;
                _selectedClassTypeNameProperty = _drawingClassTypeNameProperty;
            }

            if (_classTypeNamePropertyChanged == true)
            {
                _classTypeNamePropertyChanged = false;
                _selectedClassTypeNameProperty.stringValue = _classTypeNamePropertyValue;
                _selectedClassTypeGuidProperty.stringValue = ClassTypeReference.GetTypeGuid(ResolveType(_classTypeNamePropertyValue));
                _classTypeNamePropertyValue = string.Empty;
            }
        }

        private void DrawTypeSelectionControl(Rect position, GUIContent label,
            ClassTypeConstraintAttribute filter)
        {
            try
            {
                DrawTypeSelectionControlInternal(position, label, filter);
            }
            finally
            {
                ExcludedTypeCollectionGetter = null;
            }
        }

        private void DisplayDropDown(Rect position, List<Type> types, ClassGrouping grouping)
        {
            AdvancedTypesDropdown dropdown =
                new AdvancedTypesDropdown(types, 13, grouping, new AdvancedDropdownState());
            dropdown.Show(position);

            dropdown.NoneSelected += NoneSelectHandle;
            dropdown.TypeSelected += TypeSelectHandle;
        }

        private void TypeSelectHandle(Type type)
        {
            _classTypeNamePropertyValue = ClassTypeReference.GetClassRef(type);
            _classTypeNamePropertyChanged = true;
        }

        private void NoneSelectHandle()
        {
            _classTypeNamePropertyValue = string.Empty;
            _classTypeNamePropertyChanged = true;
        }

        #endregion

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorStyles.popup.CalcHeight(GUIContent.none, 0);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _drawingClassTypeNameProperty = property.FindPropertyRelative("_classRef");
            _drawingClassTypeGuidProperty = property.FindPropertyRelative("_typeScriptFileGuid");
            
            DrawTypeSelectionControl(position, label, attribute as ClassTypeConstraintAttribute);
        }
    }
}