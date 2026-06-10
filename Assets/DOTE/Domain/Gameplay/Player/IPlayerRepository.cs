namespace DOTE.Gameplay.Domain.Player
{
    public interface IPlayerRepository
    {
        public void AddPlayer(GamePartyPlayer player);
        public void RemovePlayer(string playerId);
        public GamePartyPlayer GetPlayerById(string playerId);
        public void Clear();
    }
}