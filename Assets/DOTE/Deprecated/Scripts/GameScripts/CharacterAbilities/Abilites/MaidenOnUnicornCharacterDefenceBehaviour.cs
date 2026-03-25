using System;
using UnityEngine;

[Serializable]
public class MaidenOnUnicornCharacterDefenceBehaviour : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private MaidenOnUnicornCharacterDefenceBehaviourData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (MaidenOnUnicornCharacterDefenceBehaviourData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.IgnorePhysDamage = true;

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        UseCard(abilityOwner.gameObject);
    }

    public void ReturnToNormal()
    {
        abilityOwner.IgnorePhysDamage = false;
        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class MaidenOnUnicornCharacterDefenceBehaviourData : BaseCharacterAbilityData
{
    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}