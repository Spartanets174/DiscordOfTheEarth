using System;
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
        public int SelectedIndex { get; private set; }

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
            SelectedIndex = -1;
            columnCount = 4;
        }

        public void ResetSelectedPrefab()
        {
            SelectedPrefab = null;
            SelectedPrefabType = null;
            SelectedIndex = -1;
        }

        public void DrawPrefabsPanel(Rect position)
        {
            // ---- Сетка префабов ----
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            float availableWidth = position.width - 20f;
            columnCount = Mathf.Max(1, Mathf.FloorToInt((availableWidth + Padding) / (ItemSize + Padding)));

            int itemCount = gamePartyScene.CellPrefabs.Count;
            if (itemCount > 0)
            {
                for (int i = 0; i < itemCount; i += columnCount)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int j = 0; j < columnCount && i + j < itemCount; j++)
                    {
                        int index = i + j;
                        ACellView prefab = gamePartyScene.CellPrefabs[index];
                        bool isSelected = (index == SelectedIndex);
                        DrawPrefabItem<ACellView>(prefab.gameObject, index, isSelected);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
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
                if (SelectedPrefab == prefab && SelectedIndex == index)
                {
                    togglePlacementMode.Invoke();
                }
                else
                {
                    SelectedPrefab = prefab;
                    SelectedPrefabType = typeof(T);
                    SelectedIndex = index;

                    enablePlacementMode.Invoke();
                }

                repaint.Invoke();
                Event.current.Use();
                SceneView.RepaintAll();
            }
        }
    }
}