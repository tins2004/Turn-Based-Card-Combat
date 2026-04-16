#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RequiredAttribute))]
public class RequiredDrawer : PropertyDrawer 
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        RequiredAttribute required = (RequiredAttribute)attribute;
        bool isEmpty = IsPropertyEmpty(property);

        if (isEmpty) 
        {
            EditorGUI.HelpBox(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, 20), required.ErrorMessage, MessageType.Error);
            GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
        }

        Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(fieldRect, property, label);

        GUI.backgroundColor = Color.white;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
    {
        if (IsPropertyEmpty(property)) 
        {
            return EditorGUIUtility.singleLineHeight + 25;
        }
        return EditorGUIUtility.singleLineHeight;
    }

    private bool IsPropertyEmpty(SerializedProperty property) 
    {
        switch (property.propertyType) 
        {
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue == null;
            case SerializedPropertyType.String:
                return string.IsNullOrEmpty(property.stringValue);
            default:
                return false;
        }
    }
}
#endif