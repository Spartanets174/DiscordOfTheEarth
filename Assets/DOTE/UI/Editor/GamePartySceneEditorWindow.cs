using DOTE.Gameplay.Infrastructure;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public class GamePartySceneEditorWindow : EditorWindow
    {
        public class GamePartySceneEditorWindowDataContainer : ScriptableObject
        {
            public Grid Grid => grid;
            public SingleplayerGamePartyConfig GamePartyConfig => gamePartyConfig;
            public List<ACellView> CellPrefabs => cellPrefabs;
            public List<AObstacleView> ObstaclePrefabs => obstaclePrefabs;

            [SerializeField]
            private Grid grid;

            [SerializeField]
            private SingleplayerGamePartyConfig gamePartyConfig;

            [SerializeField]
            private List<ACellView> cellPrefabs = new();

            [SerializeField]
            private List<AObstacleView> obstaclePrefabs = new();
        }

        private const string HexSizeLabel = "Размер гекса";
        private const string MaxNumOfCellsInDirectionLabel = "Дальность отрисовки сетки";

        private GamePartySceneEditorWindowDataContainer dataContainer;
        private GamePartySceneEditorPrefabsPanel prefabsPanel;
        private GamePartySceneEditorPrefabsSpawner prefabsSpawner;
        private SerializedObject serializedObject;

        private SerializedProperty grid;
        private SerializedProperty gamePartyConfig;
        private SerializedProperty cellPrefabs;
        private SerializedProperty obstaclePrefabs;

        [SerializeField]
        private int maxNumOfCellsInDirection = 10;
        private bool mouseDragOrDownPrevFrame;

        private float defaultLabelWidth;
        private float originalLabelWidth;

        private Type cellType;
        private Type obstacleType;
        private Type characterType;

        [MenuItem("DOTE/GamePartySceneEditorWindow")]
        public static void ShowWindow()
        {
            GamePartySceneEditorWindow window = GetWindow<GamePartySceneEditorWindow>("GamePartySceneEditor");
            window.Init();
        }

        public void Init()
        {
            dataContainer = CreateInstance<GamePartySceneEditorWindowDataContainer>();

            serializedObject = new SerializedObject(dataContainer);

            originalLabelWidth = EditorGUIUtility.labelWidth;
            defaultLabelWidth = CalculateFieldWidth(MaxNumOfCellsInDirectionLabel) + 5f;

            grid = serializedObject.FindProperty(nameof(grid));
            gamePartyConfig = serializedObject.FindProperty(nameof(gamePartyConfig));
            cellPrefabs = serializedObject.FindProperty(nameof(cellPrefabs));
            obstaclePrefabs = serializedObject.FindProperty(nameof(obstaclePrefabs));

            cellType = typeof(ACellView);
            obstacleType = typeof(AObstacleView);
            characterType = typeof(CharacterView);

            GamePartySceneEditorStyles.Init();
        }

        private void OnEnable()
        {
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
            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            GUILayout.Label("Редактор сцены партии", GamePartySceneEditorStyles.FirstLevelHeader);

            GUILayout.Space(GamePartySceneEditorStyles.DefaultSpace);

            DrawFieldEditor();
        }

        private void DrawFieldEditor()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.BeginVertical(GamePartySceneEditorStyles.BoxPaddingStyle);

            GUILayout.Label("Настройки редактора поля", GamePartySceneEditorStyles.SecondLevelHeader);

            GUILayout.Space(GamePartySceneEditorStyles.SmallSpace);

            EditorGUIUtility.labelWidth = defaultLabelWidth;

            serializedObject.Update();

            EditorGUILayout.PropertyField(grid);
            EditorGUILayout.PropertyField(gamePartyConfig);
            EditorGUILayout.PropertyField(cellPrefabs, true);
            EditorGUILayout.PropertyField(obstaclePrefabs, true);

            serializedObject.ApplyModifiedProperties();

            if (dataContainer.Grid == null)
            {
                EditorGUILayout.HelpBox("Для редактирования настроек добавьте Grid компонент", MessageType.Info);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                return;
            }

            if (prefabsPanel == null && prefabsSpawner == null)
            {
                prefabsPanel = new GamePartySceneEditorPrefabsPanel(dataContainer, Repaint, TogglePlacementMode, EnablePlacementMode);
                prefabsSpawner = new GamePartySceneEditorPrefabsSpawner(prefabsPanel, dataContainer.Grid, Repaint);
            }

            EditorGUI.BeginChangeCheck();

            prefabsSpawner.SetHexRadius(EditorGUILayout.FloatField(HexSizeLabel, dataContainer.Grid.cellSize.x));
            maxNumOfCellsInDirection = EditorGUILayout.IntField(MaxNumOfCellsInDirectionLabel, maxNumOfCellsInDirection);

            if (EditorGUI.EndChangeCheck())
            {
                dataContainer.Grid.cellSize = Vector3.one * prefabsSpawner.HexRadius;
                EditorUtility.SetDirty(dataContainer.Grid);
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

            GamePartySceneEditorTools.DrawHexGrid(dataContainer.Grid, maxNumOfCellsInDirection, color: new Color(0.4f, 0.8f, 1f, 0.5f));

            DrawCancelCaption();

            Ray ray = HandleUtility.GUIPointToWorldRay(evt.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                sceneView.Repaint();
                return false;
            }

            Vector3 targetPosition = GetTargetPosition(hit);
            Component targetView = GetTargetView(targetPosition, hit.collider);

            if (targetView != null && !prefabsSpawner.DestroyingMode && !mouseDragOrDownPrevFrame)
            {
                prefabsSpawner.SetDestroyingMode(true);
            }

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

        private Vector3 GetTargetPosition(RaycastHit hit)
        {
            bool isSelectedObstacle = prefabsPanel.SelectedPrefabType == obstacleType;
            bool isSelectedCharacter = prefabsPanel.SelectedPrefabType == characterType;

            if (isSelectedObstacle || isSelectedCharacter)
            {
                List<Vector3> cellPositions = prefabsSpawner.GetAllSpawnedCellPositions();
                if (prefabsSpawner.SpawnedCells.TryGetValue(prefabsSpawner.GetCellIndexByPos(hit.point), out ACellView cell))
                {
                    if ((cell.GetComponentInChildren<AObstacleView>() != null && isSelectedCharacter ) ||
                       (cell.GetComponentInChildren<CharacterView>() != null && isSelectedObstacle))
                    {
                        return GamePartySceneEditorTools.FindClosestNeighborByWorldPosition(cell.transform.position, dataContainer.Grid);
                    }
                    else
                    {
                        return GamePartySceneEditorTools.SnapToHexGrid(cell.transform.position, dataContainer.Grid);
                    }
                }
                else
                {
                    return GamePartySceneEditorTools.FindClosestNeighbor(cellPositions, hit.point);
                }
            }

            return GamePartySceneEditorTools.SnapToHexGrid(hit.point, dataContainer.Grid);
        }

        private Component GetTargetView(Vector3 position, Collider collider)
        {
            if (collider == null)
            {
                return null;
            }

            if (prefabsPanel.SelectedPrefabType == obstacleType || prefabsPanel.SelectedPrefabType == characterType)
            {
                if (prefabsSpawner.SpawnedCells.TryGetValue(prefabsSpawner.GetCellIndexByPos(position), out ACellView cell))
                {
                    Component targetViewInCell = cell.GetComponentInChildren(prefabsPanel.SelectedPrefabType);

                    if (targetViewInCell != null)
                    {
                        return targetViewInCell;
                    }
                }
            }

            return collider?.GetComponentInParent(prefabsPanel.SelectedPrefabType);
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