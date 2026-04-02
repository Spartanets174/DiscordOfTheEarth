namespace DOTE.Gameplay.Domain.Field
{
    public struct AttackTargetInfo
    {
        private ACell cellWithEnemy;
        private bool canAttackFromCurrent;
        private bool canAttackAfterMove;
        private ACell bestCellToMove;  // клетка, с которой лучше атаковать (текущая, если canAttackFromCurrent)
        private int moveCost;     // стоимость перемещения до этой клетки

        public AttackTargetInfo(ACell cellWithEnemy, bool canAttackFromCurrent, bool canAttackAfterMove, ACell bestCellToMove, int moveCost)
        {
            this.cellWithEnemy = cellWithEnemy;
            this.canAttackFromCurrent = canAttackFromCurrent;
            this.canAttackAfterMove = canAttackAfterMove;
            this.bestCellToMove = bestCellToMove;
            this.moveCost = moveCost;
        }

        public ACell CanAttackFromCurrent() => cellWithEnemy;
        public bool GetCanAttackFromCurrent() => canAttackFromCurrent;
        public bool GetCanAttackAfterMove() => canAttackAfterMove;
        public ACell GetBestCellToMove() => bestCellToMove;
        public int GetMoveCost() => moveCost;
    }
}