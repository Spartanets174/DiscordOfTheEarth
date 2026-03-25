using System.Linq;

public class MagicToadCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private MagicToadCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (MagicToadCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        foreach (var item in abilityOwner.DamageMultiplierByClassesDict.Keys.ToList())
        {
            if (abilityData.classToIgnoreDamage == item)
            {
                abilityOwner.DamageMultiplierByClassesDict[item] -= abilityData.damageIgnore;
            }
        }
    }

    public override void AbilityEnd(Character character)
    {
        foreach (var item in abilityOwner.DamageMultiplierByClassesDict.Keys.ToList())
        {
            if (abilityData.classToIgnoreDamage == item)
            {
                abilityOwner.DamageMultiplierByClassesDict[item] += abilityData.damageIgnore;
            }
        }
        abilityOwner.OnDeath -= AbilityEnd;
    }
}