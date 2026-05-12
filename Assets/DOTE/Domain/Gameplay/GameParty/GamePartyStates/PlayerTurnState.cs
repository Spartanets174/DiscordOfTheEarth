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

        private int defaultPointsOfActionValue;

        public event Action<PlayerTurnState> OnPOAChanged;

        public PlayerTurnState(string name, string playerId, int defaultPointsOfActionValue)
        {
            Name = name;
            PlayerId = playerId;
            this.defaultPointsOfActionValue = defaultPointsOfActionValue;
        }

        public override void MoveSelectedCharacter(GamePartyPlayer player, Hex targetCell, int moveCost)
        {
            base.MoveSelectedCharacter(player, targetCell, moveCost);

            if (!CanCharacterDoAction(player))
            {
                return;
            }

            if (moveCost > PointsOfAction)
            {
                return;
            }

            //player.MoveSelectedCharacter();
        }

        public override void AttackTargetBySelectedCharacter(GamePartyPlayer player, string targetID)
        {
            base.AttackTargetBySelectedCharacter(player, targetID);

            if (!CanCharacterDoAction(player))
            {
                return;
            }

            /*if (attacker.AttackCost.CurrentValue > PointsOfAction)
            {
                return;
            }*/

            //player.AttackTargetBySelectedCharacter();
        }

        public override void UseSelectedCharacterAbility(GamePartyPlayer player, ActiveAbilityType abilityType)
        {
            base.UseSelectedCharacterAbility(player, abilityType);

            /*if (character.UseAbilityCost.CurrentValue > PointsOfAction)
            {
                return;
            }*/
        }

        public void ResetPointsOfAction()
        {
            SetPOA(defaultPointsOfActionValue);
        }

        public void DecreasePointsOfAction(int value)
        {
            SetPOA(PointsOfAction - value);
        }

        private void SetPOA(int value)
        {
            if (value >= 0 && PointsOfAction != value)
            {
                PointsOfAction = value;
                OnPOAChanged?.Invoke(this);
            }
        }

        private bool CanCharacterDoAction(GamePartyPlayer player)
        {
            return player.PlayerId == PlayerId;
        }
    }
}