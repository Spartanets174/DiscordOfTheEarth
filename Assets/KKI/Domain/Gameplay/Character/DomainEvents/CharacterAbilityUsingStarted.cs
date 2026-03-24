namespace DOTE.Domain.DomainModel
{
    public class CharacterAbilityUsingStarted : IDomainEvent
    {
        private string characterId;
        private string abilityId;

        public CharacterAbilityUsingStarted(string characterId, string abilityId)
        {
            this.characterId = characterId;
            this.abilityId = abilityId;
        }

        public string GetCharacterId() => characterId;
        public string GetAbilityId() => abilityId;
    }
}
