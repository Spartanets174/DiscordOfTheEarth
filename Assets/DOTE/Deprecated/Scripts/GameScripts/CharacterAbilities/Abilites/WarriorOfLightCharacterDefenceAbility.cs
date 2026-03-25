using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class WarriorOfLightCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private WarriorOfLightCharacterDefenceAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (WarriorOfLightCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour("", battleSystem, abilityOwner, abilityData.range, "allowed"));

        selectCellsToAttackInRangeBehaviour = (SelectCellsWithCharactersInRangeBehaviour)CardSelectBehaviour;

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }
    private void OnSelected()
    {
        foreach (var character in selectCellsToAttackInRangeBehaviour.charactersOnCells)
        {
            character.IgnoreMagDamage = true;
        }
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);
        UseCard(abilityOwner.gameObject);
    }

    public void ReturnToNormal()
    {
        foreach (var character in selectCellsToAttackInRangeBehaviour.charactersOnCells)
        {
            character.IgnoreMagDamage = false;
        }

        OnReturnToNormal?.Invoke(this);
    }
}
[Serializable]
public class WarriorOfLightCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public int range;

    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}