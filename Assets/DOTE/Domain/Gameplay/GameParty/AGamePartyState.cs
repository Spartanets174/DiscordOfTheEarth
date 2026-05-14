using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.Player;
using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.GameParty
{
    public abstract class AGamePartyState : AState
    {
        public virtual void SelectCharacter(GamePartyPlayer player, PlayableCharacter character) { }
        public virtual void DeselectCharacter(GamePartyPlayer player, PlayableCharacter character) { }
        public virtual void MoveCharacter(GamePartyPlayer player, PlayableCharacter character, Hex targetCell, int MoveCost) { }
        public virtual void AttackCharacter(GamePartyPlayer player, PlayableCharacter attacker, PlayableCharacter target) { }
        public virtual void UseCharacterAbility(GamePartyPlayer player, PlayableCharacter character, ActiveAbilityType abilityType) { }
        public virtual void UseSupportCard(GamePartyPlayer player, ASupportCard supportCard) { }
    }
}