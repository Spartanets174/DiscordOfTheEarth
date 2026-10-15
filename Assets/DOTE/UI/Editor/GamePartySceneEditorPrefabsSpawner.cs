using System;
using UnityEditor;
using UnityEngine;
using Zenject.SpaceFighter;

namespace DOTE.Gameplay.UI
{
    public class GamePartySceneEditorPrefabsSpawner
    {
        public float HexRadius { get; private set; }

        public bool DestroyingMode { get; private set; }
        public bool PlacementMode { get; private set; }

        private GamePartySceneEditorPrefabsPanel prefabsPanel;
        private Grid grid;

        private GameObject ghostObject;
        private Material ghostCreateMaterial;
        private Material ghostDestroyMaterial;

        private Type cellType;

        private Action repaint;

        public GamePartySceneEditorPrefabsSpawner(GamePartySceneEditorPrefabsPanel prefabsPanel, Grid grid, Action repaint)
        {
            this.prefabsPanel = prefabsPanel;
            this.grid = grid;

            HexRadius = 1;
            cellType = typeof(ACellView);
            PlacementMode = false;
            DestroyingMode = false;
            this.repaint = repaint;
        }

        public void SetHexRadius(float radius)
        {
            HexRadius = radius;
        }

        public void SetGhostPostition(Vector3 postition)
        {
            if (ghostObject != null)
            {
                ghostObject.transform.position = postition;
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

        public void EnablePlacementMode()
        {
            if (prefabsPanel.SelectedPrefab == null)
            {
                return;
            }

            PlacementMode = true;

            // Создаём ghost, если его нет
            if (prefabsPanel.SelectedPrefab != null && ghostObject == null)
                CreateGhost();

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

            GameObject.DestroyImmediate(ghostCreateMaterial);
            GameObject.DestroyImmediate(ghostDestroyMaterial);
        }

        public void DestroyGhost()
        {
            if (ghostObject != null)
            {
                GameObject.DestroyImmediate(ghostObject);
                ghostObject = null;
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

            if (instance != null)
            {
                Undo.RegisterCreatedObjectUndo(instance, "Place Prefab");
                Selection.activeGameObject = instance.gameObject;
            }
            else
            {
                Debug.LogWarning("instance null");
            }           
        }

        private GameObject PlaceCellPrefab(Vector3 position, Quaternion rotation)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabsPanel.SelectedPrefab, grid.transform);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.transform.localScale = Vector3.one * HexRadius;

            return instance.gameObject;
        }

        private void CreateGhost()
        {
            if (prefabsPanel.SelectedPrefab == null) return;

            ghostObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabsPanel.SelectedPrefab);
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
            ghostObject.transform.localScale = Vector3.one * HexRadius;
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