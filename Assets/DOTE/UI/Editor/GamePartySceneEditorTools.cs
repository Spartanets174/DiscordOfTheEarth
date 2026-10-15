using UnityEditor;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public static class GamePartySceneEditorTools 
    {
        /// <summary>
        /// Преобразует мировую точку в центр ближайшей ячейки шестиугольной сетки (pointy-top).
        /// Центр сетки — позиция объекта с компонентом Grid.
        /// </summary>
        public static Vector3 SnapToHexGrid(Vector3 worldPos, Grid activeGrid)
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
        public static void DrawHexGrid(Grid grid, int radius = 10, Color? color = null, bool drawCenterMark = true)
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
    }
}