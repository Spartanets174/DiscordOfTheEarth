using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Character
{
    public class CharacterInformation
    {
        private string characterName;
        private string characterDescription;
        private Race characterRace;
        private Class characterClass;
        private Rarity characterRarity;

        public CharacterInformation(string characterName, string characterDescription, Race characterRace, Class characterClass, Rarity characterRarity)
        {
            this.characterName = characterName;
            this.characterDescription = characterDescription;
            this.characterRace = characterRace;
            this.characterClass = characterClass;
            this.characterRarity = characterRarity;
        }

        public string GetCharacterName() => characterName;
        public string GetCharacterDescription() => characterDescription;
        public Race GetCharacterRace() => characterRace;
        public Class GetCharacterClass() => characterClass;
        public Rarity GetCharacterRarity() => characterRarity;
    }
}