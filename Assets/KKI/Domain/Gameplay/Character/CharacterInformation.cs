using DOTE.Domain.SharedKernel;
using Rarity = DOTE.Domain.SharedKernel.Rarity;

namespace DOTE.Domain.Gameplay.Character
{
    public class CharacterInformation
    {
        private string CharacterName;
        private string CharacterDescription;
        private Race CharacterRace;
        private Class CharacterClass;
        private Rarity CharacterRarity;

        public CharacterInformation(string characterName, string characterDescription, Race characterRace, Class characterClass, Rarity characterRarity)
        {
            CharacterName = characterName;
            CharacterDescription = characterDescription;
            CharacterRace = characterRace;
            CharacterClass = characterClass;
            CharacterRarity = characterRarity;
        }

        public string GetCharacterName() => CharacterName;
        public string GetCharacterDescription() => CharacterDescription;
        public Race GetCharacterRace() => CharacterRace;
        public Class GetCharacterClass() => CharacterClass;
        public Rarity GetCharacterRarity() => CharacterRarity;
    }
}