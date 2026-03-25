public class LadyOnPonyCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private LadyOnPonyCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (LadyOnPonyCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        abilityOwner.AttackMultiplierByClassesDict[abilityData.classToIncreaseDamage] += abilityData.damageIncrease;
    }

    public override void AbilityEnd(Character character)
    {
        abilityOwner.AttackMultiplierByClassesDict[abilityData.classToIncreaseDamage] += abilityData.damageIncrease;

        abilityOwner.OnDeath -= AbilityEnd;
    }
}