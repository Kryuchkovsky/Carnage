using System;
using UnityEditor;
using UnityEngine;

namespace _Logic.Extensions.Attributes.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldDrawer : PropertyDrawer
    {
        private bool _hasField;
        private bool _hasProperty;
        private bool _valueIsAppropriate;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = _valueIsAppropriate ? EditorGUI.GetPropertyHeight(property) : 0;
            return height;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var conditionalAttribute = (ConditionalFieldAttribute)attribute;
            var path = conditionalAttribute.FieldName;
            var hasDots = property.propertyPath.Contains(".");

            if (hasDots)
            {
                var index = property.propertyPath.LastIndexOf(".", StringComparison.Ordinal);
                path = property.propertyPath.Substring(0, index) + "." + conditionalAttribute.FieldName;
            }
            
            var comparedField = property.serializedObject.FindProperty(path);

            if (comparedField == null)
            {
                var fixedFieldName = $"<{conditionalAttribute.FieldName}>k__BackingField";
                path = fixedFieldName;

                if (hasDots)
                {
                    var index = property.propertyPath.LastIndexOf(".", StringComparison.Ordinal);
                    path = $"{property.propertyPath.Substring(0, index)}.{fixedFieldName}";
                }
                
                comparedField = property.serializedObject.FindProperty(path);
            }
            
            var comparedFieldValue = comparedField.boxedValue;

            CheckFieldValues(conditionalAttribute, comparedFieldValue);
            
            if (_valueIsAppropriate)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private void CheckFieldValues(ConditionalFieldAttribute attribute, object value)
        {
            var type = value.GetType();
            _valueIsAppropriate = (type == typeof(string) && attribute.StringValue == value.ToString()) ||
                   (type == typeof(int) && attribute.EnumValue == Convert.ToInt32(value)) ||
                   (type == typeof(bool) && attribute.BoolCondition == (bool)value);
        }
    }
#endif
}