using System;
using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public class GamePartySceneEditorWindow : EditorWindow
    {
        private const string HexSizeLabel = "Размер гекса";
        private const string MaxNumOfCellsInDirectionLabel = "Дальность отрисовки сетки";

        private GamePartyScene gamePartyScene;
        private GamePartySceneEditorPrefabsPanel prefabsPanel;
        private GamePartySceneEditorPrefabsSpawner prefabsSpawner;


        [SerializeField]
        private int maxNumOfCellsInDirection = 10;
        private bool mouseDragOrDownPrevFrame;

        private float defaultLabelWidth;
        private float originalLabelWidth;

        private Type cellType;
        private Type obstacleType;
        private Type characterType;

        public static void ShowWindow(GamePartyScene gamePartyScene)
        {
            GamePartySceneEditorWindow window = GetWindow<GamePartySceneEditorWindow>("GamePartySceneEditor");

            if (window.gamePartyScene != gamePartyScene)
            {
                window.Init(gamePartyScene);
            }
        }

        public void Init(GamePartyScene gamePartyScene)
        {
            this.gamePartyScene = gamePartyScene;
            prefabsPanel = new GamePartySceneEditorPrefabsPanel(gamePartyScene, Repaint, TogglePlacementMode, EnablePlacementMode);
            prefabsSpawner = new GamePartySceneEditorPrefabsSpawner(prefabsPanel, gamePartyScene.Grid, Repaint);

            originalLabelWidth = EditorGUIUtility.labelWidth;
            defaultLabelWidth = CalculateFieldWidth(MaxNumOfCellsInDirectionLabel) + 5f;

            cellType = typeof(ACellView);
            obstacleType = typeof(AObstacleView);
            characterType = typeof(CharacterView);

            GamePartySceneEditorStyles.Init();
        }

        private void OnEnable()
        {
            if (gamePartyScene == null)
            {
                Init(FindObjectOfType<GamePartyScene>());
            }
            EditorApplication.update += OnEditorUpdate;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            Clear();
        }

        private void OnDestroy()
        {
            Clear();
        }

        private void Clear()
        {
            if (prefabsSpawner != null)
            {
                prefabsSpawner.Clear();
            }

            EditorApplication.update -= OnEditorUpdate;
            SceneView.duringSceneGui -= OnSceneGUI;

            prefabsPanel = null;
            prefabsSpawner = null;
        }

        private void OnEditorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (gamePartyScene == null || prefabsSpawner == null || prefabsPanel == null)
            {
                Init(FindObjectOfType<GamePartyScene>());
            }

            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            GUILayout.Label("Редактор сцены партии", GamePartySceneEditorStyles.FirstLevelHeader);

            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            DrawFieldEditor();
        }

        private void DrawFieldEditor()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.BeginVertical(GamePartySceneEditorStyles.BoxPaddingStyle);

            GUILayout.Label("Редактор поля", GamePartySceneEditorStyles.SecondLevelHeader);

            GUILayout.Space(GamePartySceneEditorStyles.SmallSpace);

            EditorGUI.BeginChangeCheck();

            EditorGUIUtility.labelWidth = defaultLabelWidth;
            prefabsSpawner.SetHexRadius(EditorGUILayout.FloatField(HexSizeLabel, gamePartyScene.Grid.cellSize.x));
            maxNumOfCellsInDirection = EditorGUILayout.IntField(MaxNumOfCellsInDirectionLabel, maxNumOfCellsInDirection);

            if (EditorGUI.EndChangeCheck())
            {
                // Записываем напрямую в компонент Grid
                gamePartyScene.Grid.cellSize = Vector3.one * prefabsSpawner.HexRadius;
                EditorUtility.SetDirty(gamePartyScene.Grid);
                SceneView.RepaintAll();
                Repaint();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.BeginVertical(GamePartySceneEditorStyles.BoxPaddingStyle);

            prefabsPanel.DrawPrefabsPanel(position);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = originalLabelWidth;
        }

        private float CalculateFieldWidth(string labelText)
        {
            return EditorStyles.label.CalcSize(new GUIContent(labelText)).x;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (prefabsSpawner == null || prefabsPanel == null)
            {
                return;
            }

            if (!TrySpawnPrefab(sceneView))
            {
                return;
            }

            sceneView.Repaint();
        }

        private bool TrySpawnPrefab(SceneView sceneView)
        {
            if (!prefabsSpawner.PlacementMode || prefabsPanel.SelectedPrefab == null)
            {
                prefabsSpawner.DestroyGhost();
                return false;
            }

            Event evt = Event.current;
            if (evt == null) return false;

            TryForceFocusOnSceneView(sceneView, evt);

            if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape)
            {
                prefabsSpawner.DisablePlacementMode();
                evt.Use();
                return false;
            }

            GamePartySceneEditorTools.DrawHexGrid(gamePartyScene.Grid, maxNumOfCellsInDirection, color: new Color(0.4f, 0.8f, 1f, 0.5f));

            DrawCancelCaption();

            Ray ray = HandleUtility.GUIPointToWorldRay(evt.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                sceneView.Repaint();
                return false;
            }

            Component targetView = GetTargetView(hit);

            if (targetView != null && !prefabsSpawner.DestroyingMode && !mouseDragOrDownPrevFrame)
            {
                prefabsSpawner.SetDestroyingMode(true);
            }

            Vector3 targetPosition = GamePartySceneEditorTools.SnapToHexGrid(hit.point, gamePartyScene.Grid);

            prefabsSpawner.SetGhostPosition(targetPosition);

            bool isMouseEvent = evt.type == EventType.MouseDrag || evt.type == EventType.MouseDown || evt.type == EventType.MouseUp || evt.type == EventType.MouseMove;

            if (!isMouseEvent)
            {
                sceneView.Repaint();
                return false;
            }

            ProcessInput(evt, targetView, targetPosition);
            return true;
        }


        private Component GetTargetView(RaycastHit hit)
        {
            if (prefabsPanel.SelectedPrefabType == obstacleType || prefabsPanel.SelectedPrefabType == characterType)
            {
                if (prefabsSpawner.SpawnedCells.TryGetValue(prefabsSpawner.GetCellIndexByPos(hit.point), out ACellView cell))
                {
                    Component targetViewInCell = cell.GetComponentInChildren(prefabsPanel.SelectedPrefabType);

                    if (targetViewInCell != null)
                    {
                        return targetViewInCell;
                    }
                }
            }

            return hit.collider?.GetComponentInParent(prefabsPanel.SelectedPrefabType);
        }

        private void DrawCancelCaption()
        {
            Handles.BeginGUI();
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.normal.textColor = Color.white;
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            GUI.Label(new Rect(10, 10, 400, 30), $"Размещение: {prefabsPanel.SelectedPrefab.name} (Esc - отмена)", style);
            Handles.EndGUI();
        }

        private void TryForceFocusOnSceneView(SceneView sceneView, Event evt)
        {
            if (prefabsSpawner.PlacementMode && prefabsPanel.SelectedPrefab != null)
            {
                if (evt.type == EventType.MouseMove)
                {
                    Selection.activeObject = null;
                    sceneView.Focus();
                }
            }
        }

        private void ProcessInput(Event evt, Component targetView, Vector3 targetPosition)
        {
            if (evt.button == 0 && !evt.alt)
            {
                bool isDragOrDown = evt.type == EventType.MouseDrag || evt.type == EventType.MouseDown;

                if (isDragOrDown)
                {
                    if (prefabsSpawner.DestroyingMode)
                    {
                        if (targetView != null)
                        {
                            prefabsSpawner.DestroyOrReplaceHoveredObject(targetView, targetPosition);
                        }
                    }
                    else if (targetView == null)
                    {
                        prefabsSpawner.PlacePrefab(prefabsPanel.SelectedPrefabType, targetPosition, Quaternion.identity);
                    }
                }
                else if (!mouseDragOrDownPrevFrame && prefabsSpawner.DestroyingMode)
                {
                    prefabsSpawner.SetDestroyingMode(false);
                }

                mouseDragOrDownPrevFrame = isDragOrDown;
                evt.Use();
            }
            else
            {
                if (prefabsSpawner.DestroyingMode && targetView == null)
                {
                    prefabsSpawner.SetDestroyingMode(false);
                }

                mouseDragOrDownPrevFrame = false;
            }
        }

        private void TogglePlacementMode()
        {
            prefabsSpawner.TogglePlacementMode();
        }

        private void EnablePlacementMode()
        {
            prefabsSpawner.EnablePlacementMode();
        }
    }
}