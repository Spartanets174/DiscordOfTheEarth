using System;
using System.Linq;
using UniRx;
using UnityEngine;

[Serializable]
public class StormCallSupportCardAbility : BaseSupportÑardAbility
{
    private SelectCellsBehaviour selectCellsBehaviour;
    private AttackAllCharactersInAreaBehaviour attackAllCharactersInAreaBehaviour;
    private StormCallSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupportÑardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (StormCallSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectCellsBehaviour(abilityData.selectCardText, battleSystem, abilityData.area, "attack"));

        SetUseCardBehaviour(new AttackAllCharactersInAreaBehaviour(abilityData.damage, battleSystem, $"\"{abilityData.supportÑardAbilityName}è\""));

        selectCellsBehaviour = (SelectCellsBehaviour)CardSelectBehaviour;
        attackAllCharactersInAreaBehaviour = (AttackAllCharactersInAreaBehaviour)UseCardBehaviour;

        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        foreach (var item in selectCellsBehaviour.highlightedCells)
        {
            item.OnClick -= selectCellsBehaviour.OnSelectedInvoke;
        }
        battleSystem.FieldController.InvokeActionOnField(selectCellsBehaviour.UnSubscribe);
        attackAllCharactersInAreaBehaviour.cellsToAttack = selectCellsBehaviour.highlightedCells.Where(x => x.GetComponentInChildren<Character>() != null).ToList();

        if (attackAllCharactersInAreaBehaviour.cellsToAttack.Count == 0)
        {
            SelectCard();
        }
        else
        {
            GameObject spawnedEffect = Instantiate(abilityData.effect.effect, Vector3.zero, Quaternion.identity, selectCellsBehaviour.clickedCell.transform);
            spawnedEffect.transform.localPosition = new Vector3(0,0.5f,0);
            UseCard(null);
        }
    }


    private void OnCardUse()
    {
        battleSystem.FieldController.TurnOnCells();
    }
}
[Serializable]
public class StormCallSupportCardAbilityData : BaseSupportÑardAbilityData
{
    public string selectCardText;
    public float damage;

    public Vector2 area;
}