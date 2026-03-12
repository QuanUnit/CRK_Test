using System;
using UnityEditor.IMGUI.Controls;

namespace ClassTypeReference.Editor
{
    public class AdvancedTypesDropdownItem : AdvancedDropdownItem
    {
        public bool IsNone => Type == default;
    
        public Type Type { get; }

        public AdvancedTypesDropdownItem(Type type, string name) : base(name)
        {
            Type = type;
        }
    }
}