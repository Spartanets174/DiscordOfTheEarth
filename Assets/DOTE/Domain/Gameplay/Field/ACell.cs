using System;

namespace DOTE.Gameplay.Domain.Field
{
    public abstract class ACell
    {
        public int DefaultMoveCost { get; private set; }
        public Hex Hex { get; private set; }

        public Type OccupierType { get; private set; }
        public string OccupierOwnerId { get; private set; }
        public bool IsOccupied => OccupierType != null;

        protected ACell(int x, int y, int z, int defaultMoveCost)
        {
            Hex = new Hex(x, y, z);
            DefaultMoveCost = defaultMoveCost;
        }

        public void Place(Type occupierType, string occupierOwnerGuid)
        {
            if (!IsOccupied)
            {
                OccupierOwnerId = occupierOwnerGuid;
                OccupierType = occupierType;
            }
        }

        public void Free()
        {
            if (IsOccupied)
            {
                OccupierOwnerId = string.Empty;
                OccupierType = null;
            }
        }
    }
}