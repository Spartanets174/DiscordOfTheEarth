using UnityEngine;

namespace DOTE.Domain.Gameplay.Character
{
    public class CharacterMoved
    {
        private string movedCharacterId;
        private Vector2 fromCellId;
        private Vector2 toCellId;

        public CharacterMoved(string movedCharacterId, Vector2 fromCellId, Vector2 toCellId)
        {
            this.movedCharacterId = movedCharacterId;
            this.fromCellId = fromCellId;
            this.toCellId = toCellId;
        }

        public string GetMovedCharacterId() => movedCharacterId;
        public Vector2 GetFromCellId() => fromCellId;
        public Vector2 GetToCellId() => toCellId;
    }
}