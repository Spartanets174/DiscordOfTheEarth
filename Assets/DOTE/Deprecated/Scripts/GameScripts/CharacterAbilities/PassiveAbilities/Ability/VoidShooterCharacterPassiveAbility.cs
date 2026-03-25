public class VoidShooterCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private VoidShooterCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (VoidShooterCharacterPassiveAbilityData)baseAbilityData;

        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        abilityOwner.PhysDamageMultiplier -= abilityData.physDamageDecreaseAmount;
        abilityOwner.MagDamageMultiplier += abilityData.magDamageIncreaseAmount;
    }

    public override void AbilityEnd(Character character)
    {
        abilityOwner.PhysDamageMultiplier += abilityData.physDamageDecreaseAmount;
        abilityOwner.MagDamageMultiplier -= abilityData.magDamageIncreaseAmount;
    }
}