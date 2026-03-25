using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Character
{
    public class CharacterActiveAbilityUsed : IDomainEvent
    {
        private string characterId;
        private ActiveAbilityType activeAbilityType;

        public CharacterActiveAbilityUsed(string characterId, ActiveAbilityType activeAbilityType)
        {
            this.characterId = characterId;
            this.activeAbilityType = activeAbilityType;
        }

        public string GetCharacterId() => characterId;
        public ActiveAbilityType GetActiveAbilityType() => activeAbilityType;
    }
}