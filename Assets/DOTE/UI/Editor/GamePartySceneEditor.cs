using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    [CustomEditor(typeof(GamePartyScene))]
    public class GamePartySceneEditor : Editor
    {
        private SerializedProperty grid;
        private SerializedProperty cellPrefabs;
        private SerializedProperty obstaclePrefabs;
        private SerializedProperty characterPrefabs;

        private void OnEnable()
        {
            grid = serializedObject.FindProperty(nameof(grid));
            cellPrefabs = serializedObject.FindProperty(nameof(cellPrefabs));
            obstaclePrefabs = serializedObject.FindProperty(nameof(obstaclePrefabs));
            characterPrefabs = serializedObject.FindProperty(nameof(characterPrefabs));
        }

        public override void OnInspectorGUI()
        {
            if (target is not GamePartyScene gamePartyScene)
            {
                return;
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(grid);
            EditorGUILayout.PropertyField(cellPrefabs, true);
            EditorGUILayout.PropertyField(obstaclePrefabs, true);
            EditorGUILayout.PropertyField(characterPrefabs, true);

            serializedObject.ApplyModifiedProperties();

            string errorMessage = null;

            if (grid.objectReferenceValue == null)
            {
                errorMessage = "Для редактирования настроек добавьте Grid компонент";
            }
            else if (cellPrefabs.arraySize <= 0)
            {
                errorMessage = "Для редактирования настроек добавьте хотя бы один префаб клетки";
            }

            using (new EditorGUI.DisabledScope(errorMessage != null))
            {
                if (GUILayout.Button("Настройки", GUILayout.Height(25)))
                {
                    GamePartySceneEditorWindow.ShowWindow(gamePartyScene);
                }
            }

            if (errorMessage != null)
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
            }
        }
    }
}