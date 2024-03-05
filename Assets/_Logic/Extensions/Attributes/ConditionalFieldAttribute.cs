using System;
using UnityEngine;

namespace _Logic.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public readonly string FieldName;
        public readonly string StringValue;
        public readonly int EnumValue = -1;
        public readonly bool BoolCondition;

        public ConditionalFieldAttribute(string fieldName, string stringValue)
        {
            FieldName = fieldName;
            StringValue = stringValue;
        }

        public ConditionalFieldAttribute(string fieldName, int enumValue)
        {
            FieldName = fieldName;
            EnumValue = enumValue;
        }

        public ConditionalFieldAttribute(string fieldName, bool boolCondition)
        {
            FieldName = fieldName;
            BoolCondition = boolCondition;
        }
    }
}