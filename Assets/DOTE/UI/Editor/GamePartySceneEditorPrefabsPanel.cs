using DOTE.Gameplay.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static DOTE.Gameplay.UI.GamePartySceneEditorWindow;

namespace DOTE.Gameplay.UI
{
    public class GamePartySceneEditorPrefabsPanel
    {
        private const float ItemSize = 80f;
        private const float Padding = 6f;
        private const string AIEnemyText = "бота-противника";
        private const string ComputerEnemyText = "противника, управляемого компьютером";

        public GameObject SelectedPrefab { get; private set; }
        public Type SelectedPrefabType { get; private set; }

        private int columnCount;
        private Vector2 scrollPos;

        private GamePartySceneEditorWindowDataContainer dataContainer;

        private Action repaint;
        private Action togglePlacementMode;
        private Action enablePlacementMode;

        public GamePartySceneEditorPrefabsPanel(GamePartySceneEditorWindowDataContainer dataContainer, Action repaint, Action togglePlacementMode, Action enablePlacementMode)
        {
            this.dataContainer = dataContainer;
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

            DrawCellPrefabs();

            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            DrawObstaclePrefabs();

            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            DrawEnemyCharacterPrefabs(true);

            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            DrawEnemyCharacterPrefabs(false);

            EditorGUILayout.EndScrollView();
        }

        private void DrawCellPrefabs()
        {
            GUILayout.Label("Доступные префабы клеток", GamePartySceneEditorStyles.SecondLevelHeader);
            GUILayout.Space(GamePartySceneEditorStyles.SmallSpace);

            List<ACellView> cellPrefabs = dataContainer.CellPrefabs.Where(x => x != null).ToList();

            if (cellPrefabs.Count == 0)
            {
                EditorGUILayout.HelpBox("Добавьте хотя бы один префаб клетки", MessageType.Info);
            }
            else
            {
                DrawPrefabs(cellPrefabs);
            }
        }

        private void DrawObstaclePrefabs()
        {
            GUILayout.Label("Доступные префабы препятствий", GamePartySceneEditorStyles.SecondLevelHeader);
            GUILayout.Space(GamePartySceneEditorStyles.SmallSpace);

            List<AObstacleView> obstaclePrefabs = dataContainer.ObstaclePrefabs.Where(x => x != null).ToList();

            if (obstaclePrefabs.Count == 0)
            {
                EditorGUILayout.HelpBox("Добавьте хотя бы один префаб препятствия", MessageType.Info);
            }
            else
            {
                DrawPrefabs(obstaclePrefabs);
            }
        }

        private void DrawEnemyCharacterPrefabs(bool isAIEnemy)
        {
            string enemyText = isAIEnemy ? AIEnemyText : ComputerEnemyText;

            GUILayout.Label($"Доступные префабы персонажей для {enemyText}", GamePartySceneEditorStyles.SecondLevelHeader);
            GUILayout.Space(GamePartySceneEditorStyles.SmallSpace);

            if (dataContainer.GamePartyConfig == null)
            {
                EditorGUILayout.HelpBox("Добавьте конфигурацию игровой партии", MessageType.Info);
                return;
            }

            object enemyPlayerConfig = null;

            if (isAIEnemy)
            {
                enemyPlayerConfig = dataContainer.GamePartyConfig.GetAIEnemyPlayerConfig();
            }
            else
            {
                enemyPlayerConfig = dataContainer.GamePartyConfig.GetComputerPlayerConfig();
            }

            if (enemyPlayerConfig == null)
            {
                EditorGUILayout.HelpBox($"Добавьте конфигурацию для {enemyText}", MessageType.Info);
                return;
            }

            List<CharacterConfig> characterConfigs = new();

            if (enemyPlayerConfig is AIEnemyPlayerConfig aiEnemyPlayerConfig)
            {
                characterConfigs = aiEnemyPlayerConfig.GetEnemyPlayerCharacters();
            }
            else if (enemyPlayerConfig is ComputerEnemyPlayerConfig computerPlayerConfig)
            {
                characterConfigs = computerPlayerConfig.GetComputerPlayerCharacters();
            }

            if (characterConfigs.Count == 0)
            {
                EditorGUILayout.HelpBox($"Добавьте в конфигурацию для {enemyText} конфигурации его персонажей", MessageType.Info);
                return;
            }

            List<CharacterView> characterViews = characterConfigs.Select(x => x.GetCharacterPrefab()).Where(x => x != null).ToList();

            if (characterViews.Count == 0)
            {
                EditorGUILayout.HelpBox($"Добавьте в конфигурацию для {enemyText} в конфигурации его персонажей префабы самих персонажей", MessageType.Info);
                return;
            }

            characterViews = characterViews.Distinct().ToList();
            DrawPrefabs(characterViews);
        }

        private void DrawPrefabs<T>(List<T> prefabs) where T : MonoBehaviour
        {
            if (prefabs.Count == 0)
            {
                return;
            }

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