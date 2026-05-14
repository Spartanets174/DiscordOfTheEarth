using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.Player;
using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Domain;
using System;
using Zenject;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class SinglePlayerGameParty : IGameParty
    {
        public string GameId { get; private set; }
        public AGamePartyState CurrentState => ssm.state as AGamePartyState;

        private SimpleStateMachine ssm;

        private StartGameState startGameState;
        private PlayerTurnState firstPlayerState;
        private PlayerTurnState secondPlayerState;
        private EndGameState endGameState;

        [Inject]
        private IDomainEventBus domainEventBus;
        public SinglePlayerGameParty(string gameId, string firstPlayerId, string secondPlayerId, int defaultPointsOfActionValue)
        {
            GameId = gameId;

            ssm = new();

            startGameState = new();
            firstPlayerState = new(nameof(firstPlayerState), firstPlayerId, defaultPointsOfActionValue);
            secondPlayerState = new(nameof(firstPlayerState), secondPlayerId, defaultPointsOfActionValue);
            endGameState = new();

            ssm.AddState(startGameState);
            ssm.AddState(firstPlayerState);
            ssm.AddState(secondPlayerState);
            ssm.AddState(endGameState);

            firstPlayerState.OnPOAChanged += OnPOAChanged;
            secondPlayerState.OnPOAChanged += OnPOAChanged;

            firstPlayerState.OnPlayerTurnStateChanged += OnPlayerTurnStateChanged;
            secondPlayerState.OnPlayerTurnStateChanged += OnPlayerTurnStateChanged;
        }

        ~SinglePlayerGameParty()
        {
            firstPlayerState.OnPOAChanged -= OnPOAChanged;
            secondPlayerState.OnPOAChanged -= OnPOAChanged;

            firstPlayerState.OnPlayerTurnStateChanged -= OnPlayerTurnStateChanged;
            secondPlayerState.OnPlayerTurnStateChanged -= OnPlayerTurnStateChanged;
        }

        public void SetStartGameState()
        {
            ssm.SetState(startGameState);
            domainEventBus.Publish(new StartGameStateSetted());
        }

        public void SetEndGameState(string loserID)
        {
            ssm.SetState(endGameState);
            domainEventBus.Publish(new EndGameStateSetted());
        }

        public void SetFirstPlayerTurnState()
        {
            ssm.SetState(firstPlayerState);
            domainEventBus.Publish(new FirstPlayerTurnStateSetted());
        }

        public void SetSecondPlayerTurnState()
        {
            ssm.SetState(secondPlayerState);
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

        public void SetPlayerTurnStateState(Type type)
        {
            if (CurrentState is PlayerTurnState playerTurnState)
            {
                playerTurnState.SetPlayerTurnState(type);
            }
        }

        public void SetPlayerTurnStateDefaultState()
        {
            if (CurrentState is PlayerTurnState playerTurnState)
            {
                playerTurnState.SetPlayerTurnState(typeof(PlayerTurnDefaultState));
            }
        }

        public void DecreasePlayerTurnPOI(int value)
        {
            if (CurrentState is PlayerTurnState playerTurnState)
            {
                playerTurnState.DecreasePointsOfAction(playerTurnState.PointsOfAction - value);
            }
        }

        public void SelectCharacter(GamePartyPlayer player, PlayableCharacter character)
        {
            CurrentState.SelectCharacter(player, character);
        }

        public void DeselectCharacter(GamePartyPlayer player, PlayableCharacter character)
        {
            CurrentState.DeselectCharacter(player, character);
        }

        public void MoveCharacter(GamePartyPlayer player, PlayableCharacter character, Hex targetCell, int MoveCost)
        {
            CurrentState.MoveCharacter(player, character, targetCell, MoveCost);
        }

        public void AttackCharacter(GamePartyPlayer player, PlayableCharacter attacker, PlayableCharacter target)
        {
            CurrentState.AttackCharacter(player, attacker, target);
        }

        public void UseCharacterAbility(GamePartyPlayer player, PlayableCharacter character, ActiveAbilityType abilityType)
        {
            CurrentState.UseCharacterAbility(player, character, abilityType);
        }

        public void UseSupportCard(GamePartyPlayer player, ASupportCard supportCard)
        {
            CurrentState.UseSupportCard(player, supportCard);
        }

        private void OnPOAChanged(PlayerTurnState state)
        {
            domainEventBus.Publish(new PointsOfActionValueChanged(state.PointsOfAction));
        }

        private void OnPlayerTurnStateChanged(PlayerTurnState state)
        {
            domainEventBus.Publish(new PlayerTurnStateChanged(state.CurrentState.GetType()));
        }
    }
}