using DOTE.SharedKernel.Domain;
using System;

namespace DOTE.Gameplay.Domain.Character
{
    public abstract class ACharacterActiveAbility
    {
        public ActiveAbilityType ActiveAbilityType => AbilityInfo.GetActiveAbilityType();
        public CharacterActiveAbilityInformation AbilityInfo { get; private set; }
        public PlayableCharacter AbilityOwner { get; private set; }
        public bool IsUsed { get; private set; }
        public bool IsUsing { get; private set; }

        public event Action OnAbilityUsed;
        protected ACharacterActiveAbility(CharacterActiveAbilityInformation abilityInfo, PlayableCharacter abilityOwner)
        {
            AbilityInfo = abilityInfo;
            this.AbilityOwner = abilityOwner;
        }

        public void UseAbility()
        {
            if (IsUsed)
            {
                return;
            }
            UseAbilityHook();
            IsUsing = true;
        }

        public void CancelUsingAbility()
        {
            if (IsUsed)
            {
                return;
            }
            CancelUsingAbilityHook();
            IsUsing = false;
        }

        protected void CompleteUsing()
        {
            IsUsed = true;
            IsUsing = false;
            OnAbilityUsed?.Invoke();
        }

        protected abstract void UseAbilityHook();
        protected abstract void CancelUsingAbilityHook();
    }
}