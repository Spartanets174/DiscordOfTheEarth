using System;

public class DarkElfArcherCharacterAttackAbility : BaseCharacterAbility
{
    private DarkElfArcherCharacterAttackAbilityData abilityData;
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData baseCharacterAbility)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (DarkElfArcherCharacterAttackAbilityData)baseCharacterAbility;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour(abilityData.selectAbilityText, battleSystem, abilityOwner, abilityData.range, "attack"));
        SetUseCardBehaviour(new FormulaAttackSelected—haracterBehaviour(abilityOwner.PhysAttack, battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

        selectCellsToAttackInRangeBehaviour = (SelectCellsWithCharactersInRangeBehaviour)CardSelectBehaviour;

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var enemyCharacter in selectCellsToAttackInRangeBehaviour.charactersOnCells)
            {
                enemyCharacter.OnClick += UseCard;
            }
        }
    }


    private void OnCardUse()
    {
        OnCancelSelection();
    }

    private void OnCancelSelection()
    {
        foreach (var enemyCharacter in selectCellsToAttackInRangeBehaviour.charactersOnCells)
        {
            enemyCharacter.OnClick -= UseCard;
        }
        battleSystem.PlayerController.SetPlayerStates(true, false);
    }
}
[Serializable]

public class DarkElfArcherCharacterAttackAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public int range;
}
