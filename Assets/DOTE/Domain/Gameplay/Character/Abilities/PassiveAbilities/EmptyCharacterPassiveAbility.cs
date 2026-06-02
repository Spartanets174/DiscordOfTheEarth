namespace DOTE.Gameplay.Domain.Character
{
    public class EmptyCharacterPassiveAbility : ACharacterPassiveAbility
    {
        public EmptyCharacterPassiveAbility(CharacterPassiveAbilityInformation abilityInfo) : base(abilityInfo)
        {
        }

        public override void RunAbility()
        {

        }

        public override void StopAbility()
        {

        }
    }
}