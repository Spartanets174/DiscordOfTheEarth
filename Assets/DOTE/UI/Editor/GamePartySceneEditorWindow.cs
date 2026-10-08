using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public class GamePartySceneEditorWindow : EditorWindow
    {
        private GamePartyScene gamePartyScene;
        private Vector2 scrollPos;
        private int columnCount = 4;          // будет пересчитываться под ширину окна
        private const float ITEM_SIZE = 80f;  // размер квадрата для префаба (включая подпись)
        private const float PADDING = 6f;

        // Для режима размещения
        private CellView selectedPrefab = null;
        private int selectedIndex = -1;
        private bool placementMode = false;
        [SerializeField]
        private float hexRadius = 1f;
        [SerializeField]
        private int maxNumOfCellsInDirection = 10;

        private bool destroyingCellMode;
        private bool mouseDragOrDownPrevFrame;

        // Ghost-объект для предпросмотра
        private GameObject ghostObject;
        private Material ghostCreateMaterial;
        private Material ghostDestroyMaterial;

        private const float defaultSpace = 12;

        public static void ShowWindow(GamePartyScene gamePartyScene)
        {
            GamePartySceneEditorWindow window = GetWindow<GamePartySceneEditorWindow>("GamePartySceneEditor");

            if (window.gamePartyScene != gamePartyScene)
            {
                window.gamePartyScene = gamePartyScene;
            }
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
            EditorApplication.update -= OnEditorUpdate;
            SceneView.duringSceneGui -= OnSceneGUI;
            if (placementMode)
                DisablePlacementMode();

            // Не забываем уничтожить материал
            DestroyImmediate(ghostCreateMaterial);
            DestroyImmediate(ghostDestroyMaterial);
        }

        private void OnEditorUpdate()
        {
            Repaint(); // обновление миниатюр
        }

        private void OnGUI()
        {
            GUILayout.Label("Редактор сцены партии", EditorStyles.largeLabel);

            GUILayout.Space(defaultSpace);

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

            hexRadius = EditorGUILayout.FloatField("Размер гекса", gamePartyScene.Grid.cellSize.x);
            maxNumOfCellsInDirection = EditorGUILayout.IntField("Дальость отрисовки сетки гексов при установке", maxNumOfCellsInDirection);

            if (EditorGUI.EndChangeCheck())
            {
                // Записываем напрямую в компонент Grid
                gamePartyScene.Grid.cellSize = Vector3.one * hexRadius;
                EditorUtility.SetDirty(gamePartyScene.Grid);
                SceneView.RepaintAll();
                Repaint();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            // ---- Сетка префабов ----
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            float availableWidth = position.width - 20f;
            columnCount = Mathf.Max(1, Mathf.FloorToInt((availableWidth + PADDING) / (ITEM_SIZE + PADDING)));

            int itemCount = gamePartyScene.CellPrefabs.Count;
            if (itemCount > 0)
            {
                for (int i = 0; i < itemCount; i += columnCount)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int j = 0; j < columnCount && i + j < itemCount; j++)
                    {
                        int index = i + j;
                        CellView prefab = gamePartyScene.CellPrefabs[index];
                        bool isSelected = (index == selectedIndex);
                        DrawPrefabItem(prefab, index, isSelected);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!placementMode || selectedPrefab == null)
            {
                if (ghostObject != null)
                    DestroyGhost();
                return;
            }

            Event evt = Event.current;
            if (evt == null) return;

            if (placementMode && selectedPrefab != null)
            {
                // Как только мышь шевельнулась над сценой — сцена становится главным окном для ввода
                if (evt.type == EventType.MouseMove)
                {
                    // Фокусируем SceneView без визуального мигания интерфейса
                    Selection.activeObject = null; // сбрасываем выделение текста, если оно залипло
                    sceneView.Focus();
                }
            }

            if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape)
            {
                DisablePlacementMode();
                evt.Use();
                return;
            }

            DrawHexGrid(gamePartyScene.Grid, maxNumOfCellsInDirection, color: new Color(0.4f, 0.8f, 1f, 0.5f));

            Handles.BeginGUI();
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.normal.textColor = Color.white;
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            GUI.Label(new Rect(10, 10, 400, 30), $"Размещение: {selectedPrefab.name} (Esc - отмена)", style);
            Handles.EndGUI();

            Ray ray = HandleUtility.GUIPointToWorldRay(evt.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1000))
            {
                sceneView.Repaint();
                return;
            }

            CellView cellView = hit.collider?.GetComponentInParent<CellView>();

            if (cellView != null && !destroyingCellMode && !mouseDragOrDownPrevFrame)
            {
                destroyingCellMode = true;
                SetGhostMaterial(ghostDestroyMaterial);
            }

            Vector3 targetPosition = SnapToHexGrid(hit.point, gamePartyScene.Grid);

            if (ghostObject != null)
            {
                ghostObject.transform.position = targetPosition;
            }

            bool isMouseEvent = evt.type == EventType.MouseDrag || evt.type == EventType.MouseDown || evt.type == EventType.MouseUp || evt.type == EventType.MouseMove;

            if (!isMouseEvent)
            {
                sceneView.Repaint();
                return;
            }

            if (evt.button == 0 && !evt.alt)
            {
                bool isDragOrDown = evt.type == EventType.MouseDrag || evt.type == EventType.MouseDown;

                if (isDragOrDown)
                {
                    if (destroyingCellMode)
                    {
                        if (cellView != null)
                        {
                            CellView sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(cellView);

                            if (sourcePrefab != null && sourcePrefab != selectedPrefab)
                            {
                                Undo.DestroyObjectImmediate(cellView.gameObject);
                                PlacePrefab(targetPosition, Quaternion.identity);
                            }
                            else
                            {
                                Undo.DestroyObjectImmediate(cellView.gameObject);
                            }    
                        }                       
                    }
                    else if(cellView == null)
                    {
                        PlacePrefab(targetPosition, Quaternion.identity);
                    }
                }
                else if (!mouseDragOrDownPrevFrame && destroyingCellMode)
                {
                    destroyingCellMode = false;
                    SetGhostMaterial(ghostCreateMaterial);
                }

                mouseDragOrDownPrevFrame = isDragOrDown;
                evt.Use();
            }
            else
            {
                if (destroyingCellMode && cellView == null)
                {
                    destroyingCellMode = false;
                    SetGhostMaterial(ghostCreateMaterial);
                }

                mouseDragOrDownPrevFrame = false;
            }    

            sceneView.Repaint();
        }

        /// <summary>
        /// Преобразует мировую точку в центр ближайшей ячейки шестиугольной сетки (pointy-top).
        /// Центр сетки — позиция объекта с компонентом Grid.
        /// </summary>
        private Vector3 SnapToHexGrid(Vector3 worldPos, Grid activeGrid)
        {
            if (activeGrid == null)
                return worldPos;

            Transform t = activeGrid.transform;

            // Радиус ячейки берём из cellSize.x (как и в DrawHexGrid)
            float R = Mathf.Max(activeGrid.cellSize.x, 0.01f);

            // Переводим точку в локальные координаты Grid
            Vector3 local = t.InverseTransformPoint(worldPos);
            float x = local.x;
            float z = local.z;

            // --- axial (q, r) для pointy-top ---
            float q = (Mathf.Sqrt(3f) / 3f * x - 1f / 3f * z) / R;
            float r = (2f / 3f * z) / R;

            // --- cube rounding ---
            float cx = q;
            float cz = r;
            float cy = -cx - cz;

            int rx = Mathf.RoundToInt(cx);
            int ry = Mathf.RoundToInt(cy);
            int rz = Mathf.RoundToInt(cz);

            float dx = Mathf.Abs(rx - cx);
            float dy = Mathf.Abs(ry - cy);
            float dz = Mathf.Abs(rz - cz);

            if (dx > dy && dx > dz)
                rx = -ry - rz;
            else if (dy > dz)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            // --- обратно в локальные x,z ---
            float outX = R * (Mathf.Sqrt(3f) * rx + Mathf.Sqrt(3f) / 2f * rz);
            float outZ = R * (1.5f * rz);

            Vector3 snappedLocal = new Vector3(outX, local.y, outZ);
            return t.TransformPoint(snappedLocal);
        }

        /// <summary>
        /// Рисует шестиугольную сетку (pointy-top) в сцене, привязанную к компоненту Grid.
        /// Центр сетки — позиция объекта с компонентом Grid.
        /// Координаты и ориентация учитывают трансформ Grid (поворот, масштаб).
        /// </summary>
        /// <param name="grid">Компонент Grid, задающий центр и размер ячейки</param>
        /// <param name="radius">Количество колец гексов вокруг центра</param>
        /// <param name="color">Цвет линий сетки</param>
        /// <param name="drawCenterMark">Рисовать ли метку в центре сетки</param>
        public void DrawHexGrid(Grid grid, int radius = 10, Color? color = null, bool drawCenterMark = true)
        {
            if (grid == null) return;

            Transform t = grid.transform;
            Vector3 origin = t.position;

            // Радиус ячейки берём из cellSize (X). Если он нулевой — используем 1.
            float R = Mathf.Max(grid.cellSize.x, 0.01f);

            // Шаги для pointy-top гексагональной сетки:
            //   по локальной X: sqrt(3) * R
            //   по локальной Z: 1.5 * R
            float hexWidth = Mathf.Sqrt(3f) * R;
            float hexHeight = 1.5f * R;

            Color prevColor = Handles.color;
            Handles.color = color ?? new Color(0.4f, 0.8f, 1f, 0.6f);

            // Предварительно вычисляем 6 углов гекса в локальных координатах (pointy-top)
            Vector3[] localCorners = new Vector3[6];
            for (int i = 0; i < 6; i++)
            {
                float angle = Mathf.Deg2Rad * (60f * i - 30f); // -30°, 30°, 90°, ...
                localCorners[i] = new Vector3(Mathf.Cos(angle) * R, 0f, Mathf.Sin(angle) * R);
            }

            // Проходим по всем гексам в заданном радиусе (axial координаты q, r)
            for (int q = -radius; q <= radius; q++)
            {
                int rMin = Mathf.Max(-radius, -q - radius);
                int rMax = Mathf.Min(radius, -q + radius);

                for (int r = rMin; r <= rMax; r++)
                {
                    // Центр гекса в локальных координатах Grid
                    float localX = hexWidth * (q + r * 0.5f);
                    float localZ = hexHeight * r;
                    Vector3 localCenter = new Vector3(localX, 0f, localZ);

                    // Переводим углы гекса в мировые координаты
                    Vector3[] worldCorners = new Vector3[6];
                    for (int i = 0; i < 6; i++)
                    {
                        worldCorners[i] = t.TransformPoint(localCenter + localCorners[i]);
                    }

                    // Рисуем 6 сторон гекса
                    for (int i = 0; i < 6; i++)
                    {
                        Handles.DrawLine(worldCorners[i], worldCorners[(i + 1) % 6]);
                    }
                }
            }

            // Отмечаем центр сетки (позицию Grid)
            if (drawCenterMark)
            {
                Handles.color = Color.yellow;
                float handleSize = HandleUtility.GetHandleSize(origin) * 0.1f;
                Handles.SphereHandleCap(0, origin, Quaternion.identity, handleSize, EventType.Repaint);

                // Ось Z (локальная) — чтобы видеть ориентацию сетки
                Handles.color = new Color(1f, 0.5f, 0f, 0.8f);
                Handles.DrawLine(origin, origin + t.forward * R * 2f);
            }

            Handles.color = prevColor;
        }


        private void PlacePrefab(Vector3 position, Quaternion rotation)
        {
            if (selectedPrefab == null) return;

            CellView instance = PrefabUtility.InstantiatePrefab(selectedPrefab, gamePartyScene.Grid.transform) as CellView;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.transform.localScale = Vector3.one * hexRadius;
            Undo.RegisterCreatedObjectUndo(instance, "Place Prefab");
            Selection.activeGameObject = instance.gameObject;
        }

        private void DrawPrefabItem(CellView prefab, int index, bool isSelected)
        {
            Rect rect = GUILayoutUtility.GetRect(ITEM_SIZE, ITEM_SIZE + 20f,
                                                 GUILayout.Width(ITEM_SIZE),
                                                 GUILayout.Height(ITEM_SIZE + 20f));
            rect.x += PADDING / 2f;
            rect.y += PADDING / 2f;
            rect.width -= PADDING;
            rect.height -= PADDING;

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
                if (selectedPrefab == prefab && selectedIndex == index)
                {
                    TogglePlacementMode();
                }
                else
                {
                    selectedPrefab = prefab;
                    selectedIndex = index;

                    EnablePlacementMode();
                }

                Repaint();
                Event.current.Use();
                SceneView.RepaintAll();
            }
        }

        private void TogglePlacementMode()
        {
            if (placementMode)
            {
                DisablePlacementMode();
            }
            else
            {
                EnablePlacementMode();
            }

        }

        private void EnablePlacementMode()
        {
            placementMode = true;

            if (selectedPrefab == null && gamePartyScene.CellPrefabs.Count > 0)
            {
                selectedPrefab = gamePartyScene.CellPrefabs[0];
                selectedIndex = 0;
            }

            // Создаём ghost, если его нет
            if (selectedPrefab != null && ghostObject == null)
                CreateGhost();

            Repaint();
            SceneView.RepaintAll();
        }

        private void DisablePlacementMode()
        {
            placementMode = false;
            selectedPrefab = null;
            selectedIndex = -1;

            Repaint();
            SceneView.RepaintAll();
        }

        private void CreateGhost()
        {
            if (selectedPrefab == null) return;

            ghostObject = (PrefabUtility.InstantiatePrefab(selectedPrefab) as CellView).gameObject;
            ghostCreateMaterial = CreateGhostMaterial(new Color(0, 1, 1, 0.3f));
            ghostDestroyMaterial = CreateGhostMaterial(new Color(1, 0, 0, 0.3f));

            SetGhostMaterial(ghostCreateMaterial);

            MonoBehaviour[] scripts = ghostObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }

            Collider[] colliders = ghostObject.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.isTrigger = true;
            }

            ghostObject.name = "Ghost";
            SetLayerRecursively(ghostObject, LayerMask.NameToLayer("Ignore Raycast"));
            ghostObject.hideFlags = HideFlags.HideAndDontSave;
            ghostObject.transform.position = Vector3.zero;
            ghostObject.transform.localScale = Vector3.one * hexRadius;
        }

        private void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (null == obj)
            {
                return;
            }

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }

        private void SetGhostMaterial(Material material)
        {
            if (ghostObject == null)
            {
                return;
            }

            Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                Material[] newMats = new Material[rend.sharedMaterials.Length];
                for (int i = 0; i < newMats.Length; i++)
                {
                    newMats[i] = material;
                }
                rend.sharedMaterials = newMats;
                rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                rend.receiveShadows = false;
            }
        }

        private void DestroyGhost()
        {
            if (ghostObject != null)
            {
                DestroyImmediate(ghostObject);
                ghostObject = null;
            }
        }

        private Material CreateGhostMaterial(Color color)
        {
            Material ghostMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

            // Настройка прозрачности для URP
            ghostMaterial.SetFloat("_Surface", 1);      // Transparent
            ghostMaterial.SetFloat("_Blend", 0);        // Alpha
            ghostMaterial.SetColor("_BaseColor", color);
            ghostMaterial.SetFloat("_ReceiveShadows", 0);
            // Для старых версий URP / Built-in
            ghostMaterial.SetFloat("_Mode", 2); // Transparent для стандартного шейдера
            ghostMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            ghostMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            ghostMaterial.SetInt("_ZWrite", 0);
            ghostMaterial.EnableKeyword("_ALPHABLEND_ON");
            ghostMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            ghostMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            return ghostMaterial;
        }
    }
}