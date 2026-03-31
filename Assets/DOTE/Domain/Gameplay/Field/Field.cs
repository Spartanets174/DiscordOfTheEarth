using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Domain;
using System.Collections.Generic;

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

        public int CalculatePathMoveCostForCharacter(List<ACell> path, PlayableCharacter character)
        {
            int cost = 0;
            foreach (var cell in path)
            {
                cost += GetMoveCostToCellForCharacter(cell, character);
            }
            return cost;
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


        public CellsForMoveAndAttack GetCellsForMoveAndAttackForCharacter(PlayableCharacter character, int remainingActionPoints)
        {
            CellsForMoveAndAttack cellsForMoveAndAttack = FindCellsForMoveAndAttackInMoveRangeDijkstrasAlgorithm(character, remainingActionPoints);
            List<ACell> cellsForAttack = new();
            return cellsForMoveAndAttack;
        }

        private CellsForMoveAndAttack FindCellsForMoveAndAttackInMoveRangeDijkstrasAlgorithm(PlayableCharacter character, int remainingActionPoints)
        {
            List<ACell> cellsForMove = new();
            List<ACell> cellsForAttack = new();

            PriorityQueue<ACell> frontier = new PriorityQueue<ACell>();

            //хранит общую стоимость перемещения от начальной точки, также называемую «полем расстояния»
            Dictionary<ACell, int> costSoFar = new Dictionary<ACell, int>();
            Dictionary<ACell, ACell> cameFromMove = new Dictionary<ACell, ACell>();
            Dictionary<ACell, ACell> cameFromAttack = new Dictionary<ACell, ACell>();

            ACell start = cellsMap[character.PositionOnField];

            Class characterClass = character.CharacterInformation.GetCharacterClass();
            bool ignoreCharactersForAttack = characterClass == Class.Archer || characterClass == Class.Magician;

            frontier.Enqueue(start, 0);
            cameFromAttack[start] = null;// для стартовой вершины предшественник не определён
            cameFromMove[start] = null; // для стартовой вершины предшественник не определён
            costSoFar[start] = 0; //перемещение в начальную точку = 0

            /*Есть ли ещё клетки до которых я могу дойти или клетки или которые находятся в зоне атаки?
                           Да: Находится ли на клетке персонаж?
                              Да: Находится ли клетка в зоне атаки от текущей (путем проверки предыдущих клеток) и может ли персонаж атаковать    
                                Да:Персонаж враг?
                                   Да: добавляю к персонажам для атаки
                                    если разрешена атака сквозь персонажей, то добавляю клетку к пути атаки
                                  Нет: добавляю к клеткам в зоне атаки
                              Нет: Могу ли дойти до клетки?
                                  Да: добавляю к клеткам, в которые могу зайти
                Нет: конец*/
            while (frontier.Count > 0)
            {
                ACell current = frontier.Dequeue();

                foreach (ACell next in GetNeighbors(current, true))
                {
                    bool isCellInAttackRange = false/* BuildPath() <= character.AttackRange.CurrentValue*/;
                    if (!character.IsAttackedOnMove && character.CanAttack && isCellInAttackRange)
                    {
                        bool isCharacterOnCell = next.OccupierType.IsAssignableFrom(typeof(PlayableCharacter));
                        if (isCharacterOnCell)
                        {
                            cellsForAttack.Add(next);
                            if (ignoreCharactersForAttack)
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                    else if (!next.IsOccupied)
                    {
                        int newCost = costSoFar[current] + GetMoveCostToCellForCharacter(next, character);
                        if (newCost <= character.Speed.CurrentValue && newCost <= remainingActionPoints && !costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                        {
                            costSoFar[next] = newCost;
                            int priority = newCost;
                            frontier.Enqueue(next, priority);
                            cameFromMove[next] = current;

                            cellsForMove.Add(next);
                        }
                    }
                }
            }

            return new(cellsForMove, cellsForAttack);
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


        private List<ACell> GetNeighbors(ACell cell, bool ignoreCharacters = false)
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
                    else if (neighbor.OccupierType.IsAssignableFrom(typeof(PlayableCharacter)) && ignoreCharacters)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        private int GetMoveCostToCellForCharacter(ACell to, PlayableCharacter character)
        {
            return character.GetCellMoveCostByCellType(to.GetType());
        }

    }
}
