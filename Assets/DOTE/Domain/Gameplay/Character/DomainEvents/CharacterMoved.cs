using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Character
{
    public class CharacterMoved : IDomainEvent
    {
        private string movedCharacterId;
        private (int, int) fromCellId;
        private (int, int) toCellId;

        public CharacterMoved(string movedCharacterId, (int, int) fromCellId, (int, int) toCellId)
        {
            this.movedCharacterId = movedCharacterId;
            this.fromCellId = fromCellId;
            this.toCellId = toCellId;
        }

        public string GetMovedCharacterId() => movedCharacterId;
        public (int, int) GetFromCellId() => fromCellId;
        public (int, int) GetToCellId() => toCellId;
    }
}