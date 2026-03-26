namespace DOTE.Gameplay.Domain.Character
{
    public abstract class ACharacterPassiveAbility
    {
        public string AbilityID { get; private set; }
        public CharacterPassiveAbilityInformation AbilityInfo { get; private set; }

        protected PlayableCharacter abilityOwner;

        protected ACharacterPassiveAbility(string abilityID, CharacterPassiveAbilityInformation abilityInfo, PlayableCharacter abilityOwner)
        {
            AbilityID = abilityID;
            AbilityInfo = abilityInfo;
            this.abilityOwner = abilityOwner;
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