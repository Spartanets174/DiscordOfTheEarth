using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Domain;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            foreach (var cellsLine in cells)
            {
                foreach (var cell in cellsLine)
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

        public List<ACell> GetCellsInRange(Hex centerCordinate, int x, int y)
        {
            List<ACell> result = new();

            if (cellsMap.TryGetValue(centerCordinate, out ACell center))
            {
                // Вычисляем границы прямоугольника
                int qMin = center.Hex.Q - x / 2;
                int qMax = center.Hex.Q + x / 2;
                int rMin = center.Hex.R - y / 2;
                int rMax = center.Hex.R + y / 2;

                // Перебираем все возможные q и r в прямоугольнике
                for (int q = qMin; q <= qMax; q++)
                {
                    for (int r = rMin; r <= rMax; r++)
                    {
                        int s = -q - r;   // Третья координата из условия q+r+s=0
                        Hex hexInRange = new(q, r, s);

                        if (cellsMap.TryGetValue(hexInRange, out ACell targetCell))
                        {
                            result.Add(targetCell);
                        }
                    }
                }
            }

            return result;
        }

        public int CalculatePathMoveCostForCharacter(List<ACell> path, PlayableCharacter character)
        {
            int cost = 0;
            foreach (var cell in path)
            {
                cost += GetMoveCostToCellForCharacter(cell, character);
            }
            return cost;
        }

        public List<AttackTargetInfo> GetPossibleCellsWithEnemiesForAttack(PlayableCharacter character, int remainingActionPoints)
        {
            Dictionary<ACell, int> cellsForMove = GetCellsForMove(character, remainingActionPoints);
            List<AttackTargetInfo> result = new List<AttackTargetInfo>();
            List<ACell> cellsWithEnemies = cellsMap.Values.Where(x => character.IsEnemy(x.OccupierOwnerId)).ToList();
            ACell characterCell = cellsMap[character.PositionOnField];

            foreach (ACell cellWithEnemy in cellsWithEnemies)
            {
                bool canAttackFromCurrent = CanAttackFromCell(characterCell, cellWithEnemy, character);

                ACell bestCell = null;
                int bestCost = 0;
                bool canAttackAfterMove = false;

                if (!canAttackFromCurrent)
                {
                    // Поиск клетки, с которой можно атаковать после перемещения
                    foreach (var cellForMove in cellsForMove)
                    {
                        ACell cell = cellForMove.Key;
                        int moveCost = cellForMove.Value;

                        // Проверяем, хватит ли ОД на атаку после перемещения
                        if (remainingActionPoints - moveCost < character.AttackCost.CurrentValue) continue;

                        //Можем ли мы атаковать с клетки, на которую мы можем переместиться
                        canAttackAfterMove = CanAttackFromCell(cell, cellWithEnemy, character);

                        if (canAttackAfterMove && moveCost < bestCost)
                        {
                            bestCost = moveCost;
                            bestCell = cell;
                        }
                    }
                }

                if (canAttackFromCurrent || canAttackAfterMove)
                {
                    result.Add(new AttackTargetInfo(
                        cellWithEnemy,
                        canAttackFromCurrent,
                        canAttackAfterMove,
                        canAttackAfterMove ? bestCell : characterCell,
                        canAttackAfterMove ? bestCost : 0));
                }
            }

            return result;
        }

        /// <summary>
        /// Выполняет поиск всех доступных клеток для передвижения алгоритмом Дейкстры
        /// </summary>
        /// <returns>Словарь клеток с ценой перемещения на каждую</returns>
        public Dictionary<ACell, int> GetCellsForMove(PlayableCharacter character, int remainingActionPoints)
        {
            PriorityQueue<ACell> frontier = new PriorityQueue<ACell>();
            Dictionary<ACell, int> costSoFar = new Dictionary<ACell, int>();

            List<ACell> cellsForMove = new();
            int maxPossibleCost = Mathf.Min(character.Speed.CurrentValue, remainingActionPoints);
            ACell startCell = cellsMap[character.PositionOnField];

            costSoFar[startCell] = 0;
            frontier.Enqueue(startCell, 0);

            while (frontier.Count > 0)
            {
                ACell current = frontier.Dequeue();

                foreach (ACell next in GetNeighbors(current))
                {
                    int newCost = costSoFar[current] + GetMoveCostToCellForCharacter(next, character);

                    if (newCost > maxPossibleCost) continue;

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        frontier.Enqueue(next, newCost);
                    }
                }
            }

            return costSoFar;
        }

        public List<ACell> GetMovePath(PlayableCharacter character, ACell goal, int remainingActionPoints)
        {
            ACell start = cellsMap[character.PositionOnField];

            //Словарь, сопоставляющий каждый узел с его предшественником в оптимальном пути.
            Dictionary<ACell, ACell> cameFrom = FindPathAStarAgorithm(character, start, goal, remainingActionPoints);

            ACell current = goal;
            List<ACell> path = new();

            //Собираем путь из полученных клеток
            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            //Переворачиваем, тк начинали с конца
            path.Reverse();

            return path;
        }

        /// <summary>
        /// Выполняет поиск пути алгоритмом A*.
        /// </summary>
        /// <returns>Словарь, сопоставляющий каждый узел с его предшественником в оптимальном пути.</returns>
        private Dictionary<ACell, ACell> FindPathAStarAgorithm(PlayableCharacter character, ACell start, ACell goal, int remainingActionPoints)
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
                    int newCost = costSoFar[current] + GetMoveCostToCellForCharacter(next, character);
                    if (newCost <= character.Speed.CurrentValue && newCost <= remainingActionPoints && (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]))
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
        /// Выполняет проверку может ли производиться атака с заданной позиции
        /// </summary>
        private bool CanAttackFromCell(ACell fromCell, ACell cellWithEnemy, PlayableCharacter character)
        {
            bool canAttackFromCell = false;
            if (fromCell.Hex.Distance(cellWithEnemy.Hex) <= character.AttackRange.CurrentValue)
            {
                if (IsLineClear(fromCell, cellWithEnemy, character))
                {
                    canAttackFromCell = true;
                }
            }
            return canAttackFromCell;
        }

        /// <summary>
        /// Проверяет, свободна ли линия атаки между двумя клетками для указанного юнита.
        /// </summary>
        private bool IsLineClear(ACell from, ACell target, PlayableCharacter character)
        {
            if (from == target) return true;

            // Получаем промежуточные клетки
            List<ACell> cellsOnLine = GetCellsLine(from, target); // включает from и target, но будем проверять только промежуточные

            foreach (ACell cellOnLine in cellsOnLine)
            {
                if (cellOnLine == from || cellOnLine == target) continue;

                if (!CanAttackTroughtCell(cellOnLine, character))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanAttackTroughtCell(ACell cell, PlayableCharacter character)
        {
            Class characterClass = character.CharacterInformation.GetCharacterClass();
            bool canAttackTroughtCharacters = characterClass == Class.Magician || characterClass == Class.Archer;

            if (cell.OccupierOwnerId == character.OwnerId)
            {
                return true;
            }
            if (cell.OccupierType.IsAssignableFrom(typeof(AObstacle)))
            {
                return false;
            }
            if (cell.OccupierType.IsAssignableFrom(typeof(PlayableCharacter)) && !canAttackTroughtCharacters)
            {
                return false;
            }
            return true;
        }

        private List<ACell> GetCellsLine(ACell from, ACell target)
        {
            List<Hex> cellsOnLine = FractionalHex.HexLinedraw(from.Hex, target.Hex);
            List<ACell> result = new();

            foreach (var cellOnLine in cellsOnLine)
            {
                if (cellsMap.TryGetValue(cellOnLine, out ACell neighbor))
                {
                    result.Add(neighbor);
                }
            }

            return result;
        }
        private List<ACell> GetNeighbors(ACell cell)
        {
            List<ACell> neighbors = new();
            List<Hex> hexNeighbors = cell.Hex.Neighbors();

            foreach (var hexNeighbor in hexNeighbors)
            {
                if (cellsMap.TryGetValue(hexNeighbor, out ACell neighbor))
                {
                    if (!neighbor.IsOccupied)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Функция, показывающая, насколько мы близки к цели с учётом минимальной стоимости перемещения
        /// </summary>
        /// <param name="a">Цель</param>
        /// <param name="b">Текущая позиция</param>
        /// <param name="minStepCost">минимальная стоимостб перемещения на клетку</param>
        private int Heuristic(Hex a, Hex b, int minStepCost)
        {
            int steps = a.Distance(b);
            return steps * minStepCost;
        }

        private int GetMoveCostToCellForCharacter(ACell to, PlayableCharacter character)
        {
            return character.GetCellMoveCostByCellType(to.GetType());
        }
    }
}
