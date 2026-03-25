using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public string GenerateCharacterId()
        {
            return Guid.NewGuid().ToString();
        }

        public PlayableCharacter GetCharacterById(string characterId)
        {
            charactersMap.TryGetValue(characterId, out PlayableCharacter character);
            return character;
        }

        public List<PlayableCharacter> GetPlayerCharacters(string playerId)
        {
            return charactersMap.Values.Where(x => x.OwnerId == playerId).ToList();
        }
    }
}