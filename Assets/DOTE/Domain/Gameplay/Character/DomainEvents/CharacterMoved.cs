using DOTE.Gameplay.Domain.Field;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Character
{
    public class CharacterMoved : IDomainEvent
    {
        private string movedCharacterId;
        private Hex fromCellId;
        private Hex toCellId;

        public CharacterMoved(string movedCharacterId, Hex fromCellId, Hex toCellId)
        {
            this.movedCharacterId = movedCharacterId;
            this.fromCellId = fromCellId;
            this.toCellId = toCellId;
        }

        public string GetMovedCharacterId() => movedCharacterId;
        public Hex GetFromCellId() => fromCellId;
        public Hex GetToCellId() => toCellId;
    }
}