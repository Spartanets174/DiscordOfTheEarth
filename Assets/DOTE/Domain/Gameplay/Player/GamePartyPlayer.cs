using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.Item;
using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Domain;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace DOTE.Gameplay.Domain.Player
{
    public class GamePartyPlayer
    {
        public string PlayerId { get; private set; }

        [Inject]
        private IDomainEventBus domainEventBus;

        private List<string> selectedCharacterIds;
        private List<string> supportCardIds;
        private List<string> collectedItemIds;

        private Dictionary<string, bool> characterAliveMap;

        public GamePartyPlayer(string playerId, List<string> characterIds, List<string> supportCardIds)
        {
            PlayerId = playerId;
            this.supportCardIds = supportCardIds;

            foreach (var characterId in characterIds)
            {
                characterAliveMap.Add(characterId, false);
            }

            selectedCharacterIds = new();
        }

        public void SelectCharacter(string characterId)
        {
            if (IsCharacterAlive(characterId))
            {
                selectedCharacterIds.Add(characterId);
                domainEventBus.Publish(new CharacterSelected(PlayerId, characterId));
            }
        }

        public void DeselectCharacter(string characterId)
        {
            if (selectedCharacterIds.Remove(characterId))
            {
                domainEventBus.Publish(new CharacterDeselected(PlayerId, characterId));
            }
        }

        public void MoveCharacter(PlayableCharacter playableCharacter, Hex targetCell, int moveCost)
        {
            if (IsCharacterAlive(playableCharacter.CharacterId) && CanManipulateCharacter(playableCharacter.CharacterId))
            {
                playableCharacter.Move(targetCell, moveCost);
            }
        }

        public void AttackCharacter(PlayableCharacter attacker, PlayableCharacter target)
        {
            if (IsCharacterAlive(attacker.CharacterId) && CanManipulateCharacter(attacker.CharacterId))
            {
                attacker.Attack(target);
            }
        }

        public void UseCharacterAbility(PlayableCharacter character, ActiveAbilityType abilityType)
        {
            if (IsCharacterAlive(character.CharacterId) && CanManipulateCharacter(character.CharacterId))
            {
                character.UseAbility(abilityType);
            }
        }

        public void UseSupportCard(ASupportCard supportCard)
        {
            if (supportCardIds.Contains(supportCard.SupportCardId))
            {
                supportCard.UseSupportCard();
            }
        }

        public void CollectItem(IItem item)
        {
            if (!collectedItemIds.Contains(item.ItemId))
            {
                collectedItemIds.Add(item.ItemId);
            }
        }

        public void SetCharacterAliveState(string character, bool alive)
        {
            if (characterAliveMap.ContainsKey(character))
            {
                if (characterAliveMap[character] != alive)
                {
                    characterAliveMap[character] = alive;
                    domainEventBus.Publish(new PlayerAliveCharactersCountChanged(PlayerId, GetAliveCharactersCount()));
                }
            }
        }

        public List<string> GetCharacterIds()
        {
            return new List<string>(characterAliveMap.Keys);
        }
        public List<string> GetSupportCardIds()
        {
            return new List<string>(supportCardIds);
        }
        public List<string> GetCollectedItemIds()
        {
            return new List<string>(collectedItemIds);
        }

        public int GetAliveCharactersCount()
        {
            return characterAliveMap.Values.Count(x => x);
        }

        public List<string> GetSelectedCharacterIds()
        {
            return new(selectedCharacterIds);
        }

        public bool HasCharacter(string characterId)
        {
            return characterAliveMap.Keys.Contains(characterId);
        }

        public bool IsCharacterAlive(string characterId)
        {
            bool alive = false;
            characterAliveMap.TryGetValue(characterId, out alive);
            return alive;
        }

        public bool CanManipulateCharacter(string CharacterId)
        {
            return selectedCharacterIds.Contains(CharacterId) && selectedCharacterIds.Count == 1;
        }
    }
}