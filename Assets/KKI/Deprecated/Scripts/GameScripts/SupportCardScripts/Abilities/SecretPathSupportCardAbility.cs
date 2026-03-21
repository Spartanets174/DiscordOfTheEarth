using System;
using UnityEngine;

[Serializable]
public class SecretPathSupportCardAbility : BaseSupport—ardAbility
{
    private SetAbiableCellsBehaviour setAbiableCellsBehaviour;
    private SecretPathSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (SecretPathSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectCardText, battleSystem));
        SetSelectCharacterBehaviour(new SetAbiableCellsBehaviour(abilityData.selectCharacterText, battleSystem));
        SetUseCardBehaviour(new MoveToCellBehaviour(battleSystem));

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
        foreach (var item in setAbiableCellsBehaviour.cellsToMove)
        {
            item.OnClick -= UseCard;
        }
        setAbiableCellsBehaviour.cellsToMove = battleSystem.FieldController.GetCellsForMove(@object.GetComponent<Character>(), abilityData.cellsCount);
    }
    private void OnSelectCharacter()
    {
        battleSystem.EnemyController.SetEnemiesState(true);
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var item in setAbiableCellsBehaviour.cellsToMove)
            {
                item.OnClick += UseCard;
            }
        }


    }

    private void OnCardUse()
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= SetCellsToMove;
            playerCharacter.OnClick -= SelectCharacter;
        }

        foreach (var item in setAbiableCellsBehaviour.cellsToMove)
        {
            item.OnClick -= UseCard;
        }
    }
}
[Serializable]
public class SecretPathSupportCardAbilityData : BaseSupport—ardAbilityData
{
    public string selectCardText;
    public string selectCharacterText;

    public int cellsCount;
}