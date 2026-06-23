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
        private GameObject selectedPrefab = null;
        private int selectedIndex = -1;
        private bool placementMode = false;

        // Ghost-объект для предпросмотра
        private GameObject ghostObject;
        private Material ghostMaterial;


        private const float defaultSpace = 12;

        public static void ShowWindow(GamePartyScene gamePartyScene)
        {
            GamePartySceneEditorWindow window = GetWindow<GamePartySceneEditorWindow>("GamePartySceneEditor");

            if (window.gamePartyScene != gamePartyScene)
            {
                window.gamePartyScene = gamePartyScene;
                window.Init();
            }
        }

        private void Init()
        {
            CreateGhostMaterial();
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

        private void Clear()
        {
            EditorApplication.update -= OnEditorUpdate;
            SceneView.duringSceneGui -= OnSceneGUI;
            if (placementMode)
                DisablePlacementMode();

            // Не забываем уничтожить материал
            DestroyImmediate(ghostMaterial);
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

            // ---- Панель инструментов (только кнопка включения режима) ----
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.FlexibleSpace();

            GUI.backgroundColor = Color.white;

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
                        GameObject prefab = gamePartyScene.CellPrefabs[index].gameObject;
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
            Ray ray = HandleUtility.GUIPointToWorldRay(evt.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                return;
            }

            Vector3Int cellPos = gamePartyScene.Grid.WorldToCell(hit.point);
            Vector3 targetPosition = gamePartyScene.Grid.GetCellCenterWorld(cellPos);

            ghostObject.transform.position = targetPosition;

            if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape)
            {
                DisablePlacementMode();
                evt.Use();
            }

            // Обработка клика для размещения
            if (evt.type == EventType.MouseDown && evt.button == 0 && !evt.alt)
            {
                PlacePrefab(targetPosition, Quaternion.identity);
                evt.Use();
            }

            // Подсказка в SceneView
            if (placementMode && selectedPrefab != null)
            {
                Handles.BeginGUI();
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.normal.textColor = Color.white;
                style.fontSize = 14;
                style.fontStyle = FontStyle.Bold;
                GUI.Label(new Rect(10, 10, 400, 30), $"Размещение: {selectedPrefab.name} (Esc - отмена)", style);
                Handles.EndGUI();
                sceneView.Repaint();
            }

            sceneView.Repaint();
        }

        private void PlacePrefab(Vector3 position, Quaternion rotation)
        {
            if (selectedPrefab == null) return;

            GameObject instance = PrefabUtility.InstantiatePrefab(selectedPrefab, gamePartyScene.Grid.transform) as GameObject;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            Undo.RegisterCreatedObjectUndo(instance, "Place Prefab");
            Selection.activeGameObject = instance;
        }

        private void DrawPrefabItem(GameObject prefab, int index, bool isSelected)
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

            Texture2D preview = AssetPreview.GetAssetPreview(prefab);
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
                selectedPrefab = gamePartyScene.CellPrefabs[0].gameObject;
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

            ghostObject = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;


            if (ghostMaterial == null)
            {
                CreateGhostMaterial();
            }

            Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                Material[] newMats = new Material[rend.sharedMaterials.Length];
                for (int i = 0; i < newMats.Length; i++)
                {
                    newMats[i] = ghostMaterial;
                }
                rend.sharedMaterials = newMats;
                rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                rend.receiveShadows = false;
            }

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

            ghostObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            ghostObject.hideFlags = HideFlags.HideAndDontSave;
            ghostObject.transform.position = Vector3.zero;
        }

        private void DestroyGhost()
        {
            if (ghostObject != null)
            {
                DestroyImmediate(ghostObject);
                ghostObject = null;
            }
        }

        private void CreateGhostMaterial()
        {
            ghostMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

            // Настройка прозрачности для URP
            ghostMaterial.SetFloat("_Surface", 1);      // Transparent
            ghostMaterial.SetFloat("_Blend", 0);        // Alpha
            ghostMaterial.SetColor("_BaseColor", new Color(0, 1, 1, 0.3f));
            ghostMaterial.SetFloat("_ReceiveShadows", 0);
            // Для старых версий URP / Built-in
            ghostMaterial.SetFloat("_Mode", 2); // Transparent для стандартного шейдера
            ghostMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            ghostMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            ghostMaterial.SetInt("_ZWrite", 0);
            ghostMaterial.EnableKeyword("_ALPHABLEND_ON");
            ghostMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            ghostMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

    }
}