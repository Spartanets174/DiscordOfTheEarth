using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application.Character
{
    public class CharacterService
    {
        private ICharacterRepository characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            this.characterRepository = characterRepository;
        }

        public void AttackCharacter(string attackerId, string attackedId)
        {
            PlayableCharacter attacker = characterRepository.GetCharacterById(attackerId);
            PlayableCharacter attacked = characterRepository.GetCharacterById(attackedId);

            attacker.Attack(attacked);
        }

        public void HealCharacter(float amount, string characterId, bool ignoreMax = false)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);
            character.Heal(amount, ignoreMax);
        }

        public void MoveCharacter(string characterId, (int, int) targetPosition, int moveCost)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);
            character.Move(targetPosition, moveCost);
        }

        public void ResetCharacter(string characterId)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);
            character.ResetCharacter();
        }

        public void RemoveCharacterDebuffs(string characterId)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);
            character.RemoveDebuffs();
        }

        public void UseActiveAbility(string characterId, ActiveAbilityType activeAbilityType)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);
            character.UseAbility(activeAbilityType);
        }

        public void CancelUsingCurrentActiveAbility(string characterId)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);
            character.CancelUsingCurrentActiveAbility();
        }

        public void ActivatePassiveAbility(string characterId)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);
            character.ActivatePassiveAbility();
        }

        public void EquipItem(string characterId, string itemId)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);

            character.EquipItem(itemId);
        }

        public void RemoveItem(string characterId, string itemId)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(characterId);

            character.RemoveItem(itemId);
        }
    }
}