using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Player
{
    public class CharacterDeselected: IDomainEvent
    {
        private string playerId;
        private string deselectedCharacterId;

        public CharacterDeselected(string playerId, string deselectedCharacterId)
        {
            this.playerId = playerId;
            this.deselectedCharacterId = deselectedCharacterId;
        }
        public string GetPlayerId() => playerId;
        public string GetDeselectedCharacterId() => deselectedCharacterId;
    }
}