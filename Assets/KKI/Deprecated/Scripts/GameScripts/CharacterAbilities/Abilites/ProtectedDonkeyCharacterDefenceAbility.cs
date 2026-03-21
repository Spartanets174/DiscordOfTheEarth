using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class ProtectedDonkeyCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    private ProtectedDonkeyCharacterDefenceAbilityData abilityData;
    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (ProtectedDonkeyCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.MagDefence += abilityData.magDefenceAmount;
        abilityOwner.PhysAttack += abilityData.physAttackAmount;
        abilityOwner.MagAttack += abilityData.magAttackAmount;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);
    }

    public void ReturnToNormal()
    {
        abilityOwner.MagDefence -= abilityData.magDefenceAmount;
        abilityOwner.PhysAttack -= abilityData.physAttackAmount;
        abilityOwner.MagAttack -= abilityData.magAttackAmount;
        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class ProtectedDonkeyCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public float physAttackAmount;

    public float magAttackAmount;

    public float magDefenceAmount;

    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}