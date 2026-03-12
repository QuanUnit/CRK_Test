using System;
using System.Collections.Generic;
using System.Linq;
using TypeReference;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ClassTypeReference.Editor
{
    public class AdvancedTypesDropdown : AdvancedDropdown
    {
        public event Action<Type> TypeSelected;
        public event Action NoneSelected;
        
        static readonly float _headerHeight = EditorGUIUtility.singleLineHeight * 2f;

        private readonly List<Type> _types;
        private readonly ClassGrouping _grouping;

        public AdvancedTypesDropdown(IEnumerable<Type> types, int maxLineCount, ClassGrouping grouping, AdvancedDropdownState state) : base(state)
        {
            _grouping = grouping;
            _types = new List<Type>(types);
            minimumSize = new Vector2(minimumSize.x, EditorGUIUtility.singleLineHeight * maxLineCount + _headerHeight);
        }
        
        private string GetFullTypePath(Type type, ClassGrouping grouping)
        {
            string name = type.FullName;

            switch (grouping)
            {
                default:
                case ClassGrouping.None:
                    return name;

                case ClassGrouping.ByNamespace:
                    return name.Replace('.', '/');
            }
        }
        
        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new AdvancedDropdownItem("Select Type");
            
            root.AddSeparator();
            root.AddChild(new AdvancedTypesDropdownItem(default, "None"));
            root.AddSeparator();

            Dictionary<string, AdvancedDropdownItem> directoriesMap = new Dictionary<string, AdvancedDropdownItem>();

            foreach (var type in _types)
            {
                string fullTypePath = GetFullTypePath(type, _grouping);

                string[] directories = fullTypePath.Split('/');

                AdvancedDropdownItem parent = root;
                
                foreach (var directory in directories.SkipLast(1))
                {
                    if (directoriesMap.ContainsKey(directory) == false)
                    {
                        AdvancedDropdownItem directoryItem = new AdvancedDropdownItem(directory);
                        parent.AddChild(directoryItem);
                        directoriesMap.Add(directory, directoryItem);
                    }

                    parent = directoriesMap[directory];
                }

                parent.AddChild(new AdvancedTypesDropdownItem(type, directories[^1]));
            }
            
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            
            if (item is AdvancedTypesDropdownItem typeItem)
            {
                if (typeItem.IsNone)
                {
                    NoneSelected?.Invoke();  
                }
                else
                {
                    TypeSelected?.Invoke(typeItem.Type);
                }
            }
        }
    }
}