using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.Item;
using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Domain;
using System.Collections.Generic;
using Zenject;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class Player
    {
        public string PlayerId { get; private set; }
        public string SelectedCharacterID { get; private set; }
        public int PointsOfAction { get; private set; }

        private int defaultPointsOfActionValue;

        [Inject]
        private IDomainEventBus domainEventBus;

        private List<string> characterIds;
        private List<string> supportCardIds;
        private List<string> collectedItemIds;

        public Player(string playerId, int defaultPointsOfActionValue, List<string> characterIds, List<string> supportCardIds)
        {
            PlayerId = playerId;
            this.defaultPointsOfActionValue = defaultPointsOfActionValue;
            this.characterIds = characterIds;
            this.supportCardIds = supportCardIds;
        }

        public void SelectCharacter(PlayableCharacter playableCharacter)
        {
            if (characterIds.Contains(playableCharacter.CharacterId) && playableCharacter.CharacterId != playableCharacter.CharacterId)
            {
                SelectedCharacterID = playableCharacter.CharacterId;
                domainEventBus.Publish(new CharacterSelected(PlayerId, SelectedCharacterID));
            }
        }

        public void MoveSelectedCharacter(PlayableCharacter playableCharacter, Hex targetCell, int moveCost)
        {
            if (moveCost > PointsOfAction)
            {
                return;
            }
            if (characterIds.Contains(playableCharacter.CharacterId) && SelectedCharacterID == playableCharacter.CharacterId)
            {
                playableCharacter.Move(targetCell, moveCost);
                //SetPointsOfAction(PointsOfAction - moveCost);
            }
        }

        public void AttackTarget(PlayableCharacter attacker, PlayableCharacter target)
        {
            if (attacker.AttackCost.CurrentValue > PointsOfAction)
            {
                return;
            }
            if (characterIds.Contains(attacker.CharacterId))
            {
                attacker.Attack(target);
                //SetPointsOfAction(PointsOfAction - attacker.AttackCost.CurrentValue);
            }
        }

        public void UseSelectedCharacterAbility(PlayableCharacter playableCharacter, ActiveAbilityType abilityType)
        {
            if (playableCharacter.UseAbilityCost.CurrentValue > PointsOfAction)
            {
                return;
            }
            if (characterIds.Contains(playableCharacter.CharacterId) && SelectedCharacterID == playableCharacter.CharacterId)
            {
                playableCharacter.UseAbility(abilityType);
                // SetPointsOfAction(PointsOfAction - playableCharacter.UseAbilityCost.CurrentValue);
            }
        }

        public void UseSupportCard(ASupportCard supportCard)
        {
            if (supportCardIds.Contains(supportCard.SupportCardId))
            {
                supportCard.UseSupportCard();
            }
        }

        public void CollectItem(AItem item)
        {
            if (!collectedItemIds.Contains(item.ItemId))
            {
                collectedItemIds.Add(item.ItemId);
            }
        }

        public List<string> GetCharacterIds()
        {
            return new List<string>(characterIds);
        }
        public List<string> GetSupportCardIds()
        {
            return new List<string>(supportCardIds);
        }
        public List<string> GetCollectedItemIds()
        {
            return new List<string>(collectedItemIds);
        }

        public void ResetPointsOfAction()
        {
            PointsOfAction = defaultPointsOfActionValue;
        }

        public void SetPointsOfAction(int value)
        {
            PointsOfAction = value;
            domainEventBus.Publish(new PointsOfActionValueChanged(PlayerId, PointsOfAction));
        }
    }
}