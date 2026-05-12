using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;
using System;

namespace DOTE.Gameplay.Domain.GameParty
{
    public abstract class AGamePartyState : IState
    {
        public abstract string Name { get; }

        public event Action OnStateEnter;
        public event Action OnStateExit;

        public virtual void SelectCharacter(GamePartyPlayer player, string characterID) { }
        public virtual void DeselectCharacter(GamePartyPlayer player, string characterID) { }
        public virtual void MoveSelectedCharacter(GamePartyPlayer player, Hex targetCell, int MoveCost) { }
        public virtual void AttackTargetBySelectedCharacter(GamePartyPlayer player, string targetID) { }
        public virtual void UseSelectedCharacterAbility(GamePartyPlayer player, ActiveAbilityType abilityType) { }
        public virtual void UseSupportCard(GamePartyPlayer player, string supportCardID) { }

        public void Enter()
        {
            OnEnterHook();
            OnStateEnter?.Invoke();
        }

        public void Exit()
        {
            OnExitHook();
            OnStateExit?.Invoke();
        }

        protected virtual void OnEnterHook()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnExitHook()
        {
            throw new NotImplementedException();
        }
    }
}