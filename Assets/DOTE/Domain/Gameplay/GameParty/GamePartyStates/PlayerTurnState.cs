using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;
using System;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class PlayerTurnState : AGamePartyState
    {
        public override string Name { get; }
        public int PointsOfAction { get; private set; }
        public string PlayerId { get; private set; }

        public PlayerTurnState CurrentState => ssm.state as PlayerTurnState;

        private SimpleStateMachine ssm;
        private int defaultPointsOfActionValue;

        public event Action<PlayerTurnState> OnPlayerTurnStateChanged;
        public event Action<PlayerTurnState> OnPOAChanged;

        public PlayerTurnState(string name, string playerId, int defaultPointsOfActionValue)
        {
            Name = name;
            PlayerId = playerId;
            this.defaultPointsOfActionValue = defaultPointsOfActionValue;
            ssm = new();

            PlayerTurnDefaultState playerTurnDefaultState = new();

            ssm.AddState(playerTurnDefaultState);
            ssm.OnStateChanged += PlayerTurnStateChanged;
        }

        ~PlayerTurnState()
        {
            ssm.OnStateChanged -= PlayerTurnStateChanged;
        }

        protected override void OnEnterHook()
        {
            base.OnEnterHook();
            ResetPointsOfAction();
        }

        public void SetPlayerTurnState(Type type)
        {
            ssm.SetState(type);
        }

        public override void SelectCharacter(GamePartyPlayer player, PlayableCharacter character)
        {
            if (!CanPlayerDoAction(player))
            {
                return;
            }

            CurrentState.SelectCharacter(player, character);
        }

        public override void DeselectCharacter(GamePartyPlayer player, PlayableCharacter character)
        {
            if (!CanPlayerDoAction(player))
            {
                return;
            }

            CurrentState.DeselectCharacter(player, character);
        }

        public override void MoveCharacter(GamePartyPlayer player, PlayableCharacter character, Hex targetCell, int moveCost)
        {
            if (!CanPlayerDoAction(player))
            {
                return;
            }

            if (moveCost > PointsOfAction)
            {
                return;
            }

            CurrentState.MoveCharacter(player, character, targetCell, moveCost);
        }

        public override void AttackCharacter(GamePartyPlayer player, PlayableCharacter attacker, PlayableCharacter target)
        {
            if (!CanPlayerDoAction(player))
            {
                return;
            }

            if (attacker.AttackCost.CurrentValue > PointsOfAction)
            {
                return;
            }

            CurrentState.AttackCharacter(player, attacker, target);
        }

        public override void UseCharacterAbility(GamePartyPlayer player, PlayableCharacter character, ActiveAbilityType abilityType)
        {
            if (character.UseAbilityCost.CurrentValue > PointsOfAction)
            {
                return;
            }

            CurrentState.UseCharacterAbility(player, character, abilityType);
        }

        public void ResetPointsOfAction()
        {
            SetPOA(defaultPointsOfActionValue);
        }

        public void DecreasePointsOfAction(int value)
        {
            SetPOA(PointsOfAction - value);
        }

        private void PlayerTurnStateChanged()
        {
            OnPlayerTurnStateChanged?.Invoke(this);
        }

        private void SetPOA(int value)
        {
            if (value >= 0 && PointsOfAction != value)
            {
                PointsOfAction = value;
                OnPOAChanged?.Invoke(this);
            }
        }

        private bool CanPlayerDoAction(GamePartyPlayer player)
        {
            return player.PlayerId == PlayerId;
        }
    }
}