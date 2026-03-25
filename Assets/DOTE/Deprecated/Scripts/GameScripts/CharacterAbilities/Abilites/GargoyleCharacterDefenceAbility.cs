using System;
using UnityEngine;

[Serializable]
public class GargoyleCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private GargoyleCharacterDefenceAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (GargoyleCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour(""));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.IsFreezed = true;
        abilityOwner.CanDamage = false;
        abilityOwner.CanUseAbilities = false;
        abilityOwner.IgnorePhysDamage = true;

        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        UseCard(abilityOwner.gameObject);
    }


    public void ReturnToNormal()
    {
        abilityOwner.IsFreezed = false;
        abilityOwner.CanDamage = true;
        abilityOwner.CanUseAbilities = true;
        abilityOwner.IgnorePhysDamage = false;

        OnReturnToNormal?.Invoke(this);
    }
}
[Serializable]
public class GargoyleCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public int turnCount;

    [Header("Не трогать!!!")]
    public bool isBuff;

}