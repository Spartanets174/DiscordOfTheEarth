using System;
using UnityEngine;

[Serializable]
public class TeleportSupportCardAbility : BaseSupport—ardAbility
{
    private SetAbiableCellsBehaviour setAbiableCellsBehaviour;
    private TeleportSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (TeleportSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectCardText, battleSystem));
        SetSelectCharacterBehaviour(new SetAbiableCellsBehaviour(abilityData.selectCharacterText, battleSystem));
        SetUseCardBehaviour(new MoveToCellBehaviour(battleSystem, 0));

        setAbiableCellsBehaviour = (SetAbiableCellsBehaviour)SelectCharacterBehaviour;

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnCancelSelection()
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= SetCellsToMove;
            playerCharacter.OnClick -= SelectCharacter;
        }
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += SetCellsToMove;
                playerCharacter.OnClick += SelectCharacter;
            }
        }

    }

    private void SetCellsToMove(GameObject @object)
    {
        foreach (var cell in setAbiableCellsBehaviour.cellsToMove)
        {
            cell.OnClick -= UseCard;
        }
        setAbiableCellsBehaviour.cellsToMove.Clear();
        battleSystem.FieldController.InvokeActionOnField(x =>
        {
            if (x.transform.childCount == 1)
            {
                setAbiableCellsBehaviour.cellsToMove.Add(x);
            }
        });
    }

    private void OnSelectCharacter()
    {
        battleSystem.EnemyController.SetEnemiesState(true);
        foreach (var cell in setAbiableCellsBehaviour.cellsToMove)
        {
            cell.OnClick += UseCard;
        }
    }

    private void OnCardUse()
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= SetCellsToMove;
            playerCharacter.OnClick -= SelectCharacter;
        }

        foreach (var cell in setAbiableCellsBehaviour.cellsToMove)
        {
            cell.OnClick -= UseCard;
        }
    }
}
[Serializable]
public class TeleportSupportCardAbilityData : BaseSupport—ardAbilityData
{
    public string selectCardText;
    public string selectCharacterText;

}