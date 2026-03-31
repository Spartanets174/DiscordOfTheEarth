using System.Collections.Generic;

namespace DOTE.Gameplay.Domain.Field
{
    public struct CellsForMoveAndAttack
    {
        private List<ACell> cellsForMove;
        private List<ACell> cellsForAttack;

        public CellsForMoveAndAttack(List<ACell> cellsForMove, List<ACell> cellsForAttack)
        {
            this.cellsForMove = cellsForMove;
            this.cellsForAttack = cellsForAttack;
        }

        public List<ACell> GetCellsForMove() => cellsForMove;
        public List<ACell> GetCellsForAttack() => cellsForAttack;

    }
}