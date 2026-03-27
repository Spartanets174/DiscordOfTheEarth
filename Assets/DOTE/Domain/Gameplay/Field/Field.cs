using DOTE.Gameplay.Domain.Character;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DOTE.Gameplay.Domain.Field
{
    public class Field
    {
        private Dictionary<Hex, ACell> cellsMap;
        private ACell[][] cells;

        public Field(float cellSize, ACell[][] cells)
        {
            this.cells = cells;

            cellsMap = new();

            foreach (var cellFirst in cells)
            {
                foreach (var cell in cellFirst)
                {
                    cellsMap.Add(cell.Hex, cell);
                }
            }
        }

        public ACell GetCell(Hex cell)
        {
            ACell targetCell = null;
            cellsMap.TryGetValue(cell, out targetCell);
            return targetCell;
        }
        public int GetMoveCost(ACell to, PlayableCharacter character)
        {
            return GetMoveCost(cellsMap[character.PositionOnField], to, character);
        }

        public int GetMoveCost(ACell from, ACell to, PlayableCharacter character)
        {
            return 0;
        }



        public List<ACell> GetMovePath(ACell start, ACell goal, PlayableCharacter character)
        {
            //Словарь, сопоставляющий каждый узел с его предшественником в оптимальном пути.
            Dictionary<ACell, ACell> cameFrom = FindPathAStarAgorithm(start, goal, character);

            ACell current = goal;
            List<ACell> path = new();

            //Собираем путь из полученных клеток
            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Add(start);
            //Переворачиваем, тк начинали с конца
            path.Reverse();
            return path;
        }

        public List<ACell> GetCellsWithCharactersToAttack(PlayableCharacter attacker)
        {
            List<ACell> cells = new();
            return cells;
        }

        public List<ACell> GetCellsInRange(Cell start, Vector2 range)
        {
            List<ACell> cells = new();
            return cells;
        }

        public List<ACell> GetNeighbors(ACell cell)
        {
            List<ACell> neighbors = new();
            List<Hex> hexNeighbors = cell.Hex.Neighbors();

            foreach (var hexNeighbor in hexNeighbors)
            {
                if (cellsMap.TryGetValue(hexNeighbor, out ACell neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Выполняет поиск пути алгоритмом A*.
        /// </summary>
        /// <returns>Словарь, сопоставляющий каждый узел с его предшественником в оптимальном пути.</returns>
        public Dictionary<ACell, ACell> FindPathAStarAgorithm(ACell start, ACell goal, PlayableCharacter character)
        {
            PriorityQueue<ACell> frontier = new PriorityQueue<ACell>();

            //каждая клетка указывает на другую клетку, откуда мы пришли
            Dictionary<ACell, ACell> cameFrom = new Dictionary<ACell, ACell>();
            //хранит общую стоимость перемещения от начальной точки, также называемую «полем расстояния»
            Dictionary<ACell, int> costSoFar = new Dictionary<ACell, int>();

            int minStepCost = character.GetMinMoveCost();

            frontier.Enqueue(start, 0);
            cameFrom[start] = null; // для стартовой вершины предшественник не определён
            costSoFar[start] = 0; //перемещение в начальную точку = 0

            while (frontier.Count > 0)
            {
                ACell current = frontier.Dequeue();

                // Если достигли цели, завершаем поиск
                if (current == goal)
                    break;

                foreach (ACell next in GetNeighbors(current))
                {
                    int newCost = costSoFar[current] + GetMoveCost(current, next, character);
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        int priority = newCost + Heuristic(goal.Hex, next.Hex, minStepCost);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            return cameFrom;
        }

        /// <summary>
        /// Функция, показывающая, насколько мы близки к цели с учётом минимально стоимости перемещения
        /// </summary>
        /// <param name="a">Цель</param>
        /// <param name="b">Текущая позиция</param>
        /// <param name="minStepCost">минимальная стоимостб перемещения на клетку</param>
        private int Heuristic(Hex a, Hex b, int minStepCost)
        {
            int steps = a.Distance(b);
            return steps * minStepCost;
        }
    }
}
