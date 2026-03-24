using System;
using UnityEngine;

namespace DOTE.Domain.Gameplay.Character
{
    public abstract class ACharacterActiveAbility : MonoBehaviour
    {
        public string AbilityID { get; private set; }
        public CharacterActiveAbilityInformation AbilityInfo { get; private set; }
        public Character AbilityOwner { get; private set; }
        public bool IsUsed { get; private set; }
        public bool IsUsing { get; private set; }

        public event Action OnAbilityUsed;
        protected ACharacterActiveAbility(string abilityID, CharacterActiveAbilityInformation abilityInfo, Character abilityOwner)
        {
            AbilityID = abilityID;
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