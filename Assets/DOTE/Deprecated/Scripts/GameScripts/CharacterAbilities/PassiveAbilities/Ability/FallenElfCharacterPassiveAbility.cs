public class FallenElfCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private FallenElfCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (FallenElfCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        abilityOwner.AttackMultiplierByRacesDict[abilityData.raceToIncreaseDamage] += abilityData.damageIncrease;
    }

    public override void AbilityEnd(Character character)
    {
        abilityOwner.AttackMultiplierByRacesDict[abilityData.raceToIncreaseDamage] -= abilityData.damageIncrease;

        abilityOwner.OnDeath -= AbilityEnd;
    }
}