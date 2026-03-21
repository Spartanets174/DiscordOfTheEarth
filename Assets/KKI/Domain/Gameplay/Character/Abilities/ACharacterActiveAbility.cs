using UnityEngine;

namespace DOTE.Domain.Gameplay.Character
{
    public abstract class ACharacterActiveAbility : MonoBehaviour
    {
        public string AbilityID { get; private set; }
        public CharacterActiveAbilityInformation AbilityInfo { get; private set; }
        public bool IsUsed { get; private set; }

        protected Character abilityOwner;

        protected ACharacterActiveAbility(string abilityID, CharacterActiveAbilityInformation abilityInfo, Character abilityOwner)
        {
            AbilityID = abilityID;
            AbilityInfo = abilityInfo;
            this.abilityOwner = abilityOwner;
        }

        public void UseAbility()
        {
            if (IsUsed)
            {
                return;
            }
            UseAbilityHook();
            IsUsed = true;
        }

        public void CancelUsingAbility()
        {
            CancelUsingAbilityHook();
            IsUsed = false;
        }

        protected abstract void UseAbilityHook();
        protected abstract void CancelUsingAbilityHook();
    }
}