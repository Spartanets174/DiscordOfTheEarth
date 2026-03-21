using System.Linq;

public class EternalGuardianCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private EternalGuardianCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (EternalGuardianCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);
        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        foreach (var item in abilityOwner.DamageMultiplierByRacesDict.Keys.ToList())
        {
            abilityOwner.DamageMultiplierByRacesDict[item] -= abilityData.damageIgnorePercent;
        }
    }

    public override void AbilityEnd(Character character)
    {
        foreach (var item in abilityOwner.DamageMultiplierByRacesDict.Keys.ToList())
        {
            abilityOwner.DamageMultiplierByRacesDict[item] += abilityData.damageIgnorePercent;
        }
        abilityOwner.OnDeath -= AbilityEnd;
    }
}