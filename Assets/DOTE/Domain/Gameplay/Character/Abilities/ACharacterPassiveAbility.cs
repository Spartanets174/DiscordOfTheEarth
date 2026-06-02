namespace DOTE.Gameplay.Domain.Character
{
    public abstract class ACharacterPassiveAbility
    {
        public CharacterPassiveAbilityInformation AbilityInfo { get; private set; }

        protected PlayableCharacter abilityOwner;

        protected ACharacterPassiveAbility(CharacterPassiveAbilityInformation abilityInfo)
        {
            AbilityInfo = abilityInfo;
        }
        public void SetAbilityOwner(PlayableCharacter abilityOwner)
        {
            if (this.abilityOwner == null)
            {
                this.abilityOwner = abilityOwner;
            }
        }

        public abstract void RunAbility();
        public abstract void StopAbility();
    }
}