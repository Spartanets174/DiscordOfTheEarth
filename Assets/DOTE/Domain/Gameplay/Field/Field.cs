namespace DOTE.Gameplay.Domain.Field
{
    public class Field
    {
        private ACell[,] cells;

        public Field(ACell[,] cells)
        {
            this.cells = cells;
        }

        public ACell GetCell((int, int) coorinates)
        {
            int i = coorinates.Item1;
            int j = coorinates.Item2;
            if (i < 0 || i >= cells.GetLength(0) || j < 0 || j >= cells.GetLength(1))
            {
                return null;
            }
            return cells[i, j];
        }

        public int GetMoveCost((int, int) from, (int, int) to)
        {
            return 0;
        }
    }
}