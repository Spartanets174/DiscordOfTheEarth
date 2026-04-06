
namespace DOTE.Gameplay.Domain.GameParty
{
    public interface IGameParty
    {
        public string GameId { get; }
        public Player FirstPlayer { get; }
        public Player SecondPlayer { get; }
        public abstract void SetStartGameState();
        public abstract void SetEndGameState(string winnerID);
        public abstract void SetFirstPlayerTurnState();
        public abstract void SetSecondPlayerTurnState();
    }
}