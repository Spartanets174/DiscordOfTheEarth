using DOTE.Gameplay.Domain.Character;
using System.Collections.Generic;

namespace DOTE.Gameplay.Infrastructure.Character
{
    public class CharacterRepository : ICharacterRepository
    {
        private Dictionary<string, PlayableCharacter> charactersMap;

        public CharacterRepository()
        {
            charactersMap = new();
        }

        public void AddCharacter(PlayableCharacter character)
        {
            if (!charactersMap.ContainsKey(character.CharacterId))
            {
                charactersMap.Add(character.CharacterId, character);
            }
        }
        public void RemoveCharacter(string characterId)
        {
            if (charactersMap.ContainsKey(characterId))
            {
                charactersMap.Remove(characterId);
            }
        }

        public PlayableCharacter GetCharacterById(string characterId)
        {
            charactersMap.TryGetValue(characterId, out PlayableCharacter character);
            return character;
        }
    }
}