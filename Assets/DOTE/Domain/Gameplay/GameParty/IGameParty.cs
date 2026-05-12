namespace DOTE.Gameplay.Domain.GameParty
{
    public interface IGameParty
    {
        public string GameId { get; }

        public AGamePartyState CurrentState { get; }
        public void SetStartGameState();
        public void SetEndGameState(string loserID);
        public void SetFirstPlayerTurnState();
        public void SetSecondPlayerTurnState();

        public void ChangePlayerTurnState();

        public void DecreasePlayerTurnPOI(int value);
    }
}