using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class CharacterSelected: IDomainEvent
    {
        private string playerId;
        private string selectedCharacterId;

        public CharacterSelected(string playerId, string selectedCharacterId)
        {
            this.playerId = playerId;
            this.selectedCharacterId = selectedCharacterId;
        }
        public string GetPlayerId() => playerId;
        public string GetSelectedCharacterId() => selectedCharacterId;
    }
}