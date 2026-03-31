using System;

namespace DOTE.Gameplay.Domain.Field
{
    public abstract class ACell
    {
        public Hex Hex { get; private set; }

        public Type OccupierType { get; private set; }
        public string OccupierOwnerGuid { get; private set; }
        public bool IsOccupied => OccupierType != null;

        protected ACell(int x, int y, int z)
        {
            Hex = new Hex(x, y, z);
        }

        public void Place(Type occupierType, string occupierOwnerGuid)
        {
            if (!IsOccupied)
            {
                OccupierOwnerGuid = occupierOwnerGuid;
                OccupierType = occupierType;
            }
        }

        public void Free()
        {
            if (IsOccupied)
            {
                OccupierOwnerGuid = string.Empty;
                OccupierType = null;
            }
        }
    }
}