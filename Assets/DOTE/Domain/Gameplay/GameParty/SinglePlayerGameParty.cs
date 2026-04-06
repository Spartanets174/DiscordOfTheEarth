namespace DOTE.Gameplay.Domain.GameParty
{
    public class SinglePlayerGameParty : IGameParty
    {
        public string GameId { get; private set; }
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set; }

        public SinglePlayerGameParty(string gameId, Player firstPlayer, Player secondPlayer)
        {
            GameId = gameId;
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
        }

        public void SetFirstPlayerTurnState()
        {

        }

        public void SetSecondPlayerTurnState()
        {

        }

        public void SetStartGameState()
        {

        }

        public void SetEndGameState(string winnerID)
        {

        }
    }
}