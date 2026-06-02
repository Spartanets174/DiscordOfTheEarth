namespace DOTE.Gameplay.Domain.Character
{
    public class EmptyCharacterActiveAbility : ACharacterActiveAbility
    {
        public EmptyCharacterActiveAbility(CharacterActiveAbilityInformation abilityInfo) : base(abilityInfo)
        {
        }

        protected override void UseAbilityHook()
        {
            CompleteUsing();
        }
    }
}