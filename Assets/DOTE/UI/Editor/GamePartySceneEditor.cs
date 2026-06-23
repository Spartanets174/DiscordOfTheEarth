using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    [CustomEditor(typeof(GamePartyScene))]
    public class GamePartySceneEditor : Editor
    {
        private SerializedProperty grid;
        private SerializedProperty cellPrefabs;


        private void OnEnable()
        {
            grid = serializedObject.FindProperty(nameof(grid));
            cellPrefabs = serializedObject.FindProperty(nameof(cellPrefabs));
        }

        public override void OnInspectorGUI()
        {
            if (target is not GamePartyScene gamePartyScene)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(grid);
            EditorGUILayout.PropertyField(cellPrefabs);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            string errorMessage = null;

            if (gamePartyScene.Grid == null)
            {
                errorMessage = "Для редактирования настроек добавьте Grid компонент";
            }
            else if (gamePartyScene.CellPrefabs.Count <= 0)
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