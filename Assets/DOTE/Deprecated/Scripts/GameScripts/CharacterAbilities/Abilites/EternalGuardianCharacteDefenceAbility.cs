using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class EternalGuardianCharacteDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    private EternalGuardianCharacteDefenceAbilityData abilityData;

    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (EternalGuardianCharacteDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.PhysDefence += abilityData.physDefenceAmount;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);


        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);

    }

    public void ReturnToNormal()
    {
        abilityOwner.PhysDefence -= abilityData.physDefenceAmount;
        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class EternalGuardianCharacteDefenceAbilityData: BaseCharacterAbilityData
{
    public float physDefenceAmount;

    public int turnCount;

    [Header("НЕ ТРОГАТЬ")]
    public bool isBuff;
}