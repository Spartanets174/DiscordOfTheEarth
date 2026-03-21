using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class LightningStrikeSupportCardAbility : BaseSupportÑardAbility
{

    private SelectCellsBehaviour selectCellsBehaviour;
    private AttackAllCharactersInAreaBehaviour attackAllCharactersInAreaBehaviour;
    private LightningStrikeSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupportÑardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (LightningStrikeSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectCellsBehaviour(abilityData.selectCardText, battleSystem, abilityData.area, "attack"));

        SetUseCardBehaviour(new AttackAllCharactersInAreaBehaviour(abilityData.damage, battleSystem, $"{abilityData.supportÑardAbilityName}"));

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
            spawnedEffect.transform.localPosition = new Vector3(0, 0.5f, 0);
            UseCard(null);
        }
    }


    private void OnCardUse()
    {
        foreach (var item in selectCellsBehaviour.highlightedCells)
        {
            item.OnClick -= selectCellsBehaviour.OnSelectedInvoke;
        }
        battleSystem.FieldController.TurnOnCells();
    }
}
[Serializable]
public class LightningStrikeSupportCardAbilityData : BaseSupportÑardAbilityData
{
    public string selectCardText;

    public float damage;

    public Vector2 area;
}