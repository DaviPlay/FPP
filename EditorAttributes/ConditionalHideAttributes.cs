using System;
using UnityEngine;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

namespace EditorAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        public readonly string ConditionalSourceField = "";
        public const string ConditionalSourceField2 = "";
        public readonly string[] ConditionalSourceFields = { };
        public readonly bool[] ConditionalSourceFieldInverseBools = { };
        public readonly bool HideInInspector;
        public readonly bool Inverse;
        public const bool UseOrLogic = false;

        public readonly bool InverseCondition1 = false;
        public readonly bool InverseCondition2 = false;


        // Use this for initialization
        public ConditionalHideAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = false;
            Inverse = false;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse = false)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            Inverse = inverse;
        }

        public ConditionalHideAttribute(string[] conditionalSourceFields, bool[] conditionalSourceFieldInverseBools, bool hideInInspector, bool inverse)
        {
            ConditionalSourceFields = conditionalSourceFields;
            ConditionalSourceFieldInverseBools = conditionalSourceFieldInverseBools;
            HideInInspector = hideInInspector;
            Inverse = inverse;
        }

        public ConditionalHideAttribute(string[] conditionalSourceFields, bool hideInInspector, bool inverse)
        {
            ConditionalSourceFields = conditionalSourceFields;        
            HideInInspector = hideInInspector;
            Inverse = inverse;
        }
    }
}
