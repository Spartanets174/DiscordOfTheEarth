using DOTE.SharedKernel.Domain;
using Zenject;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class SinglePlayerGameParty : AStateMachine, IGameParty
    {
        public string GameId { get; private set; }
        public AGamePartyState CurrentState => state as AGamePartyState;

        private StartGameState startGameState;
        private PlayerTurnState firstPlayerState;
        private PlayerTurnState secondPlayerState;
        private EndGameState endGameState;

        [Inject]
        private IDomainEventBus domainEventBus;
        public SinglePlayerGameParty(string gameId, string firstPlayerId, string secondPlayerId, int defaultPointsOfActionValue)
        {
            GameId = gameId;

            startGameState = new();
            firstPlayerState = new(nameof(firstPlayerState), firstPlayerId, defaultPointsOfActionValue);
            secondPlayerState = new(nameof(firstPlayerState), secondPlayerId, defaultPointsOfActionValue);
            endGameState = new();

            firstPlayerState.OnPOAChanged += OnPOAChanged;
            secondPlayerState.OnPOAChanged += OnPOAChanged;
        }

        ~SinglePlayerGameParty()
        {
            firstPlayerState.OnPOAChanged -= OnPOAChanged;
            secondPlayerState.OnPOAChanged -= OnPOAChanged;
        }

        public void SetStartGameState()
        {
            SetState(startGameState);
            domainEventBus.Publish(new StartGameStateSetted());
        }

        public void SetEndGameState(string loserID)
        {
            SetState(endGameState);
            domainEventBus.Publish(new EndGameStateSetted());
        }

        public void SetFirstPlayerTurnState()
        {
            SetState(firstPlayerState);
            domainEventBus.Publish(new FirstPlayerTurnStateSetted());
        }

        public void SetSecondPlayerTurnState()
        {
            SetState(secondPlayerState);
            domainEventBus.Publish(new SecondPlayerTurnStateSetted());
        }

        public void ChangePlayerTurnState()
        {
            if (CurrentState == firstPlayerState)
            {
                SetSecondPlayerTurnState();
            }
            else
            {
                SetFirstPlayerTurnState();
            }
        }

        public void DecreasePlayerTurnPOI(int value)
        {
            if (CurrentState is PlayerTurnState playerTurnState)
            {
                playerTurnState.DecreasePointsOfAction(playerTurnState.PointsOfAction - value);
            }
        }

        private void OnPOAChanged(PlayerTurnState state)
        {
            domainEventBus.Publish(new PointsOfActionValueChanged(state.PointsOfAction));
        }
    }
}