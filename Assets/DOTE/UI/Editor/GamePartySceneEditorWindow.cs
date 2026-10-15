using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public class GamePartySceneEditorWindow : EditorWindow
    {
        private const float DefaultSpace = 12;

        private GamePartyScene gamePartyScene;
        private GamePartySceneEditorPrefabsPanel prefabsPanel;
        private GamePartySceneEditorPrefabsSpawner prefabsSpawner;


        [SerializeField]
        private int maxNumOfCellsInDirection = 10;
        private bool mouseDragOrDownPrevFrame;


        public static void ShowWindow(GamePartyScene gamePartyScene)
        {
            GamePartySceneEditorWindow window = GetWindow<GamePartySceneEditorWindow>("GamePartySceneEditor");

            if (window.gamePartyScene != gamePartyScene)
            {
                window.gamePartyScene = gamePartyScene;
                window.Init();
            }
        }

        public void Init()
        {
            prefabsPanel = new GamePartySceneEditorPrefabsPanel(gamePartyScene, Repaint, TogglePlacementMode, EnablePlacementMode);
            prefabsSpawner = new GamePartySceneEditorPrefabsSpawner(prefabsPanel, gamePartyScene.Grid, Repaint);
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
            prefabsSpawner.Clear();

            EditorApplication.update -= OnEditorUpdate;
            SceneView.duringSceneGui -= OnSceneGUI;

            prefabsPanel = null;
            prefabsSpawner = null;
        }

        private void OnEditorUpdate()
        {
            Repaint(); // обновление миниатюр
        }

        private void OnGUI()
        {
            GUILayout.Label("Редактор сцены партии", EditorStyles.largeLabel);

            GUILayout.Space(DefaultSpace);

            DrawFieldEditor();
        }

        private void DrawFieldEditor()
        {
            GUILayout.Label("Редактор поля", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUIStyle myPaddingStyle = new GUIStyle();
            myPaddingStyle.padding = new RectOffset(5, 5, 5, 5);
            EditorGUILayout.BeginVertical(myPaddingStyle);

            GUI.backgroundColor = Color.white;

            EditorGUI.BeginChangeCheck();

            prefabsSpawner.SetHexRadius(EditorGUILayout.FloatField("Размер гекса", gamePartyScene.Grid.cellSize.x));
            maxNumOfCellsInDirection = EditorGUILayout.IntField("Дальность отрисовки сетки гексов при установке", maxNumOfCellsInDirection);

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

            prefabsPanel.DrawPrefabsPanel(position);
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

            TryForceFoucsOnSceneView(sceneView, evt);

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

            Component targetViewType = hit.collider?.GetComponentInParent(prefabsPanel.SelectedPrefabType);

            if (targetViewType != null && !prefabsSpawner.DestroyingMode && !mouseDragOrDownPrevFrame)
            {
                prefabsSpawner.SetDestroyingMode(true);
            }

            Vector3 targetPosition = GamePartySceneEditorTools.SnapToHexGrid(hit.point, gamePartyScene.Grid);
            prefabsSpawner.SetGhostPostition(targetPosition);

            bool isMouseEvent = evt.type == EventType.MouseDrag || evt.type == EventType.MouseDown || evt.type == EventType.MouseUp || evt.type == EventType.MouseMove;

            if (!isMouseEvent)
            {
                sceneView.Repaint();
                return false;
            }

            ProcessInput(evt, targetViewType, targetPosition);
            return true;
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

        private void TryForceFoucsOnSceneView(SceneView sceneView, Event evt)
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

        private void ProcessInput(Event evt, Component targetViewType, Vector3 targetPosition)
        {
            if (evt.button == 0 && !evt.alt)
            {
                bool isDragOrDown = evt.type == EventType.MouseDrag || evt.type == EventType.MouseDown;

                if (isDragOrDown)
                {
                    if (prefabsSpawner.DestroyingMode)
                    {
                        if (targetViewType != null)
                        {
                            DestroyOrReplaceHoveredObject(targetViewType, targetPosition);
                        }
                    }
                    else if (targetViewType == null)
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
                if (prefabsSpawner.DestroyingMode && targetViewType == null)
                {
                    prefabsSpawner.SetDestroyingMode(false);
                }

                mouseDragOrDownPrevFrame = false;
            }
        }

        private void DestroyOrReplaceHoveredObject(Component targetViewType, Vector3 targetPosition)
        {
            Component sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(targetViewType);

            if (sourcePrefab != null && sourcePrefab.gameObject != prefabsPanel.SelectedPrefab)
            {
                Undo.DestroyObjectImmediate(targetViewType.gameObject);
                prefabsSpawner.PlacePrefab(prefabsPanel.SelectedPrefabType, targetPosition, Quaternion.identity);
            }
            else
            {
                Undo.DestroyObjectImmediate(targetViewType.gameObject);
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