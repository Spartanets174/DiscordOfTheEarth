using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public class GamePartySceneEditorPrefabsPanel
    {
        private const float ItemSize = 80f;
        private const float Padding = 6f;

        public GameObject SelectedPrefab { get; private set; }
        public Type SelectedPrefabType { get; private set; }

        private int columnCount;
        private Vector2 scrollPos;

        private GamePartyScene gamePartyScene;

        private Action repaint;
        private Action togglePlacementMode;
        private Action enablePlacementMode;

        public GamePartySceneEditorPrefabsPanel(GamePartyScene gamePartyScene, Action repaint, Action togglePlacementMode, Action enablePlacementMode)
        {
            this.gamePartyScene = gamePartyScene;
            this.repaint = repaint;
            this.togglePlacementMode = togglePlacementMode;
            this.enablePlacementMode = enablePlacementMode;

            SelectedPrefab = null;
            SelectedPrefabType = null;
            columnCount = 4;
        }

        public void ResetSelectedPrefab()
        {
            SelectedPrefab = null;
            SelectedPrefabType = null; ;
        }

        public void DrawPrefabsPanel(Rect position)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            float availableWidth = position.width - 20f;
            columnCount = Mathf.Max(1, Mathf.FloorToInt((availableWidth + Padding) / (ItemSize + Padding)));

            GUILayout.Label("Доступные префабы клеток", GamePartySceneEditorStyles.SecondLevelHeader);
            GUILayout.Space(GamePartySceneEditorStyles.SmallSpace);
            DrawPrefabs(gamePartyScene.CellPrefabs);

            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            GUILayout.Label("Доступные префабы препятствий", GamePartySceneEditorStyles.SecondLevelHeader);
            GUILayout.Space(GamePartySceneEditorStyles.SmallSpace);
            DrawPrefabs(gamePartyScene.ObstaclePrefabs);

            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            GUILayout.Label("Доступные префабы персонажей", GamePartySceneEditorStyles.SecondLevelHeader);
            GUILayout.Space(GamePartySceneEditorStyles.SmallSpace);
            DrawPrefabs(gamePartyScene.CharacterPrefabs);

            EditorGUILayout.EndScrollView();
        }

        private void DrawPrefabs<T>(List<T> prefabs) where T : MonoBehaviour
        {
            int itemCount = prefabs.Count;
            int selectedPrefabIndex = -1;
            List<GameObject> gameObjects = prefabs.Select(x => x.gameObject).ToList();
            if (gameObjects.Contains(SelectedPrefab))
            {
                selectedPrefabIndex = gameObjects.IndexOf(SelectedPrefab);
            }

            for (int i = 0; i < itemCount; i += columnCount)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < columnCount && i + j < itemCount; j++)
                {
                    int index = i + j;
                    T prefab = prefabs[index];
                    bool isSelected = (index == selectedPrefabIndex);
                    DrawPrefabItem<T>(prefab.gameObject, index, isSelected);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawPrefabItem<T>(GameObject prefab, int index, bool isSelected)
        {
            Rect rect = GUILayoutUtility.GetRect(ItemSize, ItemSize + 20f,
                                                 GUILayout.Width(ItemSize),
                                                 GUILayout.Height(ItemSize + 20f));
            rect.x += Padding / 2f;
            rect.y += Padding / 2f;
            rect.width -= Padding;
            rect.height -= Padding;

            // Фон
            if (isSelected)
            {
                EditorGUI.DrawRect(rect, new Color(0.3f, 0.6f, 1f, 0.3f));
            }

            GUI.Box(rect, GUIContent.none, EditorStyles.helpBox);

            // Миниатюра
            Rect thumbnailRect = new Rect(rect.x + 4, rect.y + 4, rect.width - 8, rect.width - 8);
            Rect labelRect = new Rect(rect.x, rect.y + rect.width - 4, rect.width, 18);

            Texture2D preview = AssetPreview.GetAssetPreview(prefab.gameObject);
            if (preview != null)
                GUI.DrawTexture(thumbnailRect, preview);
            else
                GUI.DrawTexture(thumbnailRect, EditorGUIUtility.IconContent("GameObject Icon").image);

            GUI.Label(labelRect, prefab.name, EditorStyles.miniLabel);

            // Обработка клика для выбора префаба
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                if (SelectedPrefab == prefab)
                {
                    togglePlacementMode.Invoke();
                }
                else
                {
                    SelectedPrefab = prefab;
                    SelectedPrefabType = typeof(T);

                    enablePlacementMode.Invoke();
                }

                repaint.Invoke();
                Event.current.Use();
                SceneView.RepaintAll();
            }
        }
    }
}