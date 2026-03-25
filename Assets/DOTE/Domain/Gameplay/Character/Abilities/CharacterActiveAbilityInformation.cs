using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Character
{
    public class CharacterActiveAbilityInformation
    {
        private string abilityName;
        private string abilityDescription;
        private ActiveAbilityType activeAbilityType;

        public CharacterActiveAbilityInformation(string abilityName, string abilityDescription, ActiveAbilityType activeAbilityType)
        {
            this.abilityName = abilityName;
            this.abilityDescription = abilityDescription;
            this.activeAbilityType = activeAbilityType;
        }

        public string GetAbilityName() => abilityName;
        public string GetAbilityDescription() => abilityDescription;
        public ActiveAbilityType GetActiveAbilityType() => activeAbilityType;
    }
}