namespace DOTE.Gameplay.Domain.Character
{
    public class CharacterPassiveAbilityInformation
    {
        private string abilityName;
        private string abilityDescription;

        public CharacterPassiveAbilityInformation(string abilityName, string abilityDescription)
        {
            this.abilityName = abilityName;
            this.abilityDescription = abilityDescription;
        }

        public string GetAbilityName() => abilityName;
        public string GetAbilityDescription() => abilityDescription;
    }
}