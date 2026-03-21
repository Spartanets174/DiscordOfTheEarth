namespace DOTE.Domain.Gameplay.Character
{
    public class CharacterAbilityUsed
    {
        private string characterId;
        private string abilityId;

        public CharacterAbilityUsed(string characterId, string abilityId)
        {
            this.characterId = characterId;
            this.abilityId = abilityId;
        }

        public string GetCharacterId() => characterId;
        public string GetAbilityId() => abilityId;
    }
}