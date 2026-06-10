using DOTE.Gameplay.Domain.Player;
using System.Collections.Generic;

namespace DOTE.Gameplay.Infrastructure
{
    public class PlayerRepository : IPlayerRepository
    {
        private Dictionary<string, GamePartyPlayer> playersMap;

        public PlayerRepository()
        {
            playersMap = new();
        }

        public void AddPlayer(GamePartyPlayer player)
        {
            playersMap.TryAdd(player.PlayerId, player);
        }

        public GamePartyPlayer GetPlayerById(string playerId)
        {
            playersMap.TryGetValue(playerId, out GamePartyPlayer player);
            return player;
        }

        public void RemovePlayer(string playerId)
        {
            playersMap.Remove(playerId);
        }
        public void Clear()
        {
            playersMap.Clear();
        }
    }
}