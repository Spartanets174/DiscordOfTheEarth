public class PeasantWithPitchforkCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private PeasantWithPitchforkCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (PeasantWithPitchforkCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        abilityOwner.IgnoreMoveCostTroughtSwamp = true;
    }

    public override void AbilityEnd(Character character)
    {
        abilityOwner.IgnoreMoveCostTroughtSwamp = false;

        abilityOwner.OnDeath -= AbilityEnd;
    }
}