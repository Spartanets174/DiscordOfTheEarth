using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public class GamePartySceneEditorPrefabsSpawner
    {
        public float HexRadius { get; private set; }

        public bool DestroyingMode { get; private set; }
        public bool PlacementMode { get; private set; }

        public Dictionary<Vector2Int, ACellView> SpawnedCells { get; private set; }

        private GamePartySceneEditorPrefabsPanel prefabsPanel;
        private Grid grid;

        private GameObject ghostObject;
        private Type ghostObjectType;
        private Material ghostCreateMaterial;
        private Material ghostDestroyMaterial;

        private Type cellType;
        private Type obstacleType;
        private Type characterType;

        private Action repaint;

        public GamePartySceneEditorPrefabsSpawner(GamePartySceneEditorPrefabsPanel prefabsPanel, Grid grid, Action repaint)
        {
            this.prefabsPanel = prefabsPanel;
            this.grid = grid;
            this.repaint = repaint;

            HexRadius = 1;

            cellType = typeof(ACellView);
            obstacleType = typeof(AObstacleView);
            characterType = typeof(CharacterView);

            PlacementMode = false;
            DestroyingMode = false;
            
            SpawnedCells = new();

            foreach (ACellView aCellView in grid.GetComponentsInChildren<ACellView>())
            {
                SpawnedCells.Add(GetCellIndexByPos(aCellView.transform.position), aCellView);
            }
        }

        public void SetHexRadius(float radius)
        {
            HexRadius = radius;
        }

        public void SetGhostPosition(Vector3 position)
        {
            if (ghostObject != null)
            {
                ghostObject.transform.position = position;
            }
        }

        public void TogglePlacementMode()
        {
            if (PlacementMode)
            {
                DisablePlacementMode();
            }
            else
            {
                EnablePlacementMode();
            }
        }

        public void SetDestroyingMode(bool destroyingMode)
        {
            DestroyingMode = destroyingMode;
            SetGhostMaterial(DestroyingMode ? ghostDestroyMaterial : ghostCreateMaterial);
        }

        public Vector2Int GetCellIndexByPos(Vector3 position)
        {
            Vector3Int worldPos = grid.WorldToCell(GamePartySceneEditorTools.SnapToHexGrid(position, grid));
            return new(worldPos.x, worldPos.z);
        }

        public void EnablePlacementMode()
        {
            if (prefabsPanel.SelectedPrefab == null)
            {
                return;
            }

            PlacementMode = true;

            if (prefabsPanel.SelectedPrefab != null)
            {
                if (ghostObject == null)
                {
                    CreateGhost();
                }
                else if (ghostObjectType != prefabsPanel.SelectedPrefabType)
                {
                    DestroyGhost();
                    CreateGhost();
                }
            }

            repaint.Invoke();
            SceneView.RepaintAll();
        }

        public void DisablePlacementMode()
        {
            PlacementMode = false;
            prefabsPanel.ResetSelectedPrefab();

            repaint.Invoke();
            SceneView.RepaintAll();
        }

        public void Clear()
        {
            if (PlacementMode)
                DisablePlacementMode();

            DestroyGhost();
            GameObject.DestroyImmediate(ghostCreateMaterial);
            GameObject.DestroyImmediate(ghostDestroyMaterial);
        }

        public void DestroyGhost()
        {
            if (ghostObject != null)
            {
                GameObject.DestroyImmediate(ghostObject);
                ghostObject = null;
                ghostObjectType = null;
            }
        }

        public void DestroyOrReplaceHoveredObject(Component targetView, Vector3 targetPosition)
        {
            Component sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(targetView);

            if (sourcePrefab != null && sourcePrefab.gameObject != prefabsPanel.SelectedPrefab)
            {
                DestroyPrefab(targetView);
                PlacePrefab(prefabsPanel.SelectedPrefabType, targetPosition, Quaternion.identity);
            }
            else
            {
                DestroyPrefab(targetView);
            }
        }

        public void PlacePrefab(Type prefabType, Vector3 position, Quaternion rotation)
        {
            if (prefabsPanel.SelectedPrefab == null) return;

            GameObject instance = null;

            if (prefabType == cellType)
            {
                instance = PlaceCellPrefab(position, rotation);
            }

            if (prefabType == obstacleType || prefabType == characterType)
            {
                if (SpawnedCells.TryGetValue(GetCellIndexByPos(position), out ACellView cell))
                {
                    instance = PlacePrefab(cell.transform);
                }
            }

            if (instance != null)
            {
                Undo.RegisterCreatedObjectUndo(instance, "Place Prefab");
                Selection.activeGameObject = instance.gameObject;
            }
            else
            {
                Debug.LogWarning("Создать не получилось!");
            }
        }

        private GameObject PlaceCellPrefab(Vector3 position, Quaternion rotation)
        {
            Vector2Int index = GetCellIndexByPos(position);

            if (SpawnedCells.ContainsKey(index))
            {
                Debug.LogWarning("Невозможно установить клетку, тк это место уже занято!");
                return null;
            }

            GameObject instance = InstantiatePrefab(grid.transform);

            instance.transform.position = position;
            instance.transform.rotation = rotation;

            instance.transform.GetChild(0).localScale = Vector3.one * HexRadius;

            SpawnedCells.TryAdd(index, instance.GetComponent<ACellView>());

            return instance.gameObject;
        }

        private GameObject PlacePrefab(Transform parentCell)
        {
            GameObject instance = InstantiatePrefab(parentCell);

            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;

            return instance.gameObject;
        }

        private GameObject InstantiatePrefab(Transform parent)
        {
            return (GameObject)PrefabUtility.InstantiatePrefab(prefabsPanel.SelectedPrefab, parent);
        }

        private void DestroyPrefab(Component targetViewType)
        {
            if (targetViewType is ACellView aCellView)
            {
                SpawnedCells.Remove(GetCellIndexByPos(aCellView.transform.position));
            }

            Undo.DestroyObjectImmediate(targetViewType.gameObject);
        }

        private void CreateGhost()
        {
            if (prefabsPanel.SelectedPrefab == null) return;

            ghostObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabsPanel.SelectedPrefab);
            ghostObjectType = prefabsPanel.SelectedPrefabType;
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

            if (prefabsPanel.SelectedPrefabType == cellType)
            {
                ghostObject.transform.localScale = Vector3.one * HexRadius;
            }
            else
            {
                ghostObject.transform.localScale = Vector3.one;
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
    }
}