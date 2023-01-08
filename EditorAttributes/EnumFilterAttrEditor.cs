using Gun_Stuff;
using UnityEditor;
using UnityEngine;

namespace EditorAttributes
{
    [CustomPropertyDrawer(typeof(MyEnumFilterAttribute))]
    public class EnumFilterAttrEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MyEnumFilterAttribute filterAttribute = (MyEnumFilterAttribute) attribute;
            property.intValue = EditorGUI.IntPopup(position, label, property.intValue, System.Array.ConvertAll(filterAttribute.Labels, l => new GUIContent(l)), filterAttribute.Values);
        }
    }
}