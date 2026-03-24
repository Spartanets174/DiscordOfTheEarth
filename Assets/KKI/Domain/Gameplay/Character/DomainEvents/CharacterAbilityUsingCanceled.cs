namespace DOTE.Domain.DomainModel
{
    public class CharacterAbilityUsingCanceled : IDomainEvent
    {
        private string characterId;
        private string abilityId;

        public CharacterAbilityUsingCanceled(string characterId, string abilityId)
        {
            this.characterId = characterId;
            this.abilityId = abilityId;
        }

        public string GetCharacterId() => characterId;
        public string GetAbilityId() => abilityId;
    }
}