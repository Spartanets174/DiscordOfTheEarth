using System;
using UnityEngine;

[Serializable]
public class GuardianOfFreePrisonersCharacterAttackAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    private Character character;

    public event Action<ITurnCountable> OnReturnToNormal;

    private int decreaseAmount;

    private GuardianOfFreePrisonersCharacterAttackAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character abilityOwner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = abilityOwner;
        this.battleSystem = battleSystem;
        abilityData = (GuardianOfFreePrisonersCharacterAttackAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllEnemyUnitsBehaviour("Выберите вражеского персонажа для атаки", battleSystem));
        SetSelectCharacterBehaviour(new SetCurrentEnemyCharacterBehaviour("", battleSystem));
        SetUseCardBehaviour(new AttackSelectedСharacterBehaviour(abilityData.damage, battleSystem, "\"Стальные оковы\""));

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var enemyCharacter in battleSystem.EnemyController.EnemyCharObjects)
            {
                enemyCharacter.OnClick += SelectCharacter;
            }
        }

    }


    private void OnSelectCharacter()
    {
        if (battleSystem.State is PlayerTurn)
        {
            character = battleSystem.EnemyController.CurrentEnemyCharacter;
        }
        else
        {
            character = battleSystem.PlayerController.CurrentPlayerCharacter;
        }

        decreaseAmount = (int)Math.Ceiling(character.Speed * abilityData.speedDecreaseAmount);
        character.Speed = decreaseAmount;

        UseCard(character.gameObject);
    }

    private void OnCardUse()
    {
        OnCancelSelection();
    }

    private void OnCancelSelection()
    {
        battleSystem.EnemyController.SetEnemiesStates(true, false, x =>
        {
            x.OnClick -= SelectCharacter;
        });
        battleSystem.PlayerController.SetPlayerStates(true, false);
    }
    public void ReturnToNormal()
    {
        character.Speed += decreaseAmount;
        OnReturnToNormal?.Invoke(this);
    }
}
[Serializable]
public class GuardianOfFreePrisonersCharacterAttackAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float damage;

    public float speedDecreaseAmount;

    public int turnCount;

    [Header("Не трогать!!!")]
    public bool isBuff;
}