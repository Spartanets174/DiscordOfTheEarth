using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DOTE.SharedKernel.Domain
{
    [Serializable]
    public class GuidProperty
    {
        public string guidString;
        public bool lockGuid;

        private int m_EditorInstanceId;
        private Guid editingGuid;

        public Guid ToGuid() => string.IsNullOrEmpty(guidString) ? Guid.Empty : Guid.Parse(guidString);

        public void Regenerate(UnityEngine.Object unityObject, string propertyName)
        {
            if (lockGuid)
                return;

            Guid guid = System.Guid.NewGuid();

#if UNITY_EDITOR
        SerializedObject ownerObject = new SerializedObject(unityObject);
        SerializedProperty propertyProperty = ownerObject.FindProperty(propertyName);
        SerializedProperty guidStringProperty = propertyProperty.FindPropertyRelative(nameof(guidString));
        guidStringProperty.stringValue = guid.ToString();
        ownerObject.ApplyModifiedProperties();
#endif

            editingGuid = guid;
            m_EditorInstanceId = unityObject.GetInstanceID();
        }

        public void Update(UnityEngine.Object unityObject, string propertyName)
        {
            if (lockGuid)
                return;

            Guid guid = string.IsNullOrEmpty(guidString) ? Guid.Empty : Guid.Parse(guidString);

            // was duplicated (with Ctrl+D)
            MonoBehaviour monoBehaviour = unityObject as MonoBehaviour;
            if (monoBehaviour != null)
            {
                if (monoBehaviour.gameObject.scene.isLoaded && guid != Guid.Empty && m_EditorInstanceId != unityObject.GetInstanceID())
                    guid = Guid.Empty;
            }

            ScriptableObject scriptableObject = unityObject as ScriptableObject;
            if (scriptableObject != null)
            {
                if (guid != Guid.Empty && m_EditorInstanceId != unityObject.GetInstanceID())
                    guid = Guid.Empty;
            }

            // reset or revert prefab overrides
            if (guid == Guid.Empty && editingGuid != Guid.Empty)
                guid = editingGuid;

            if (guid == Guid.Empty)
            {
                guid = System.Guid.NewGuid();
                guidString = guid.ToString();
            }

            editingGuid = guid;
            m_EditorInstanceId = unityObject.GetInstanceID();

#if UNITY_EDITOR
        SerializedObject ownerObject = new SerializedObject(unityObject);
        SerializedProperty propertyProperty = ownerObject.FindProperty(propertyName);
        SerializedProperty guidStringProperty = propertyProperty.FindPropertyRelative(nameof(guidString));

        guidStringProperty.stringValue = guid.ToString();
        ownerObject.ApplyModifiedProperties();

        propertyProperty.Dispose();
        guidStringProperty.Dispose();
        ownerObject.Dispose();
#endif
        }
    }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(GuidProperty))]
public class GuidPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GuidProperty instance = fieldInfo.GetValue(property.serializedObject.targetObject) as GuidProperty;

        SerializedProperty lockGuid = property.FindPropertyRelative("lockGuid");
        string lockButtonTitle = lockGuid.boolValue ? "unlock" : "lock";
        GUIContent lockButtonContent = lockGuid.boolValue ? EditorGUIUtility.IconContent("IN LockButton on") : EditorGUIUtility.IconContent("IN LockButton");
        GUIContent updateButtonContent = EditorGUIUtility.IconContent("d_Refresh");

        EditorGUI.BeginProperty(position, label, property);

        var rect1 = new Rect(position.x, position.y, 30, 20);
        var rect2 = new Rect(position.x + 30, position.y, position.width - 30 * 2, 20);
        var rect3 = new Rect(position.x + position.width - 30, position.y, 30, 20);

        EditorGUI.BeginDisabledGroup(lockGuid.boolValue);
        EditorGUI.LabelField(rect2, instance.guidString, EditorStyles.helpBox);
        EditorGUI.EndDisabledGroup();
        if (GUI.Button(rect1, lockButtonContent))
        {
            lockGuid.boolValue = !lockGuid.boolValue;
        }
        EditorGUI.BeginDisabledGroup(lockGuid.boolValue);
        if (GUI.Button(rect3, updateButtonContent))
        {
            instance.Regenerate(property.serializedObject.targetObject, property.name);
        }
        EditorGUI.EndDisabledGroup();

        EditorGUI.EndProperty();
    }
}
#endif

}