using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Player
{
    public class PlayerAliveCharactersCountChanged : IDomainEvent
    {
        private string playerId;
        private int count;

        public PlayerAliveCharactersCountChanged(string playerId, int count)
        {
            this.playerId = playerId;
            this.count = count;
        }

        public string GetPlayerId() => playerId;
        public int GetCount() => count;
    }
}