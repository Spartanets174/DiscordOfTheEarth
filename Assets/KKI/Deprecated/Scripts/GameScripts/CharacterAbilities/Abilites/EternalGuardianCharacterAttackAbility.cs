using System;
using UnityEngine;

[Serializable]
public class EternalGuardianCharacterAttackAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    private Character character;
    private EternalGuardianCharacterAttackAbilityData abilityData;

    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (EternalGuardianCharacterAttackAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllEnemyUnitsBehaviour(abilityData.selectAbilityText, battleSystem));
        SetSelectCharacterBehaviour(new SetCurrentEnemyCharacterBehaviour("", battleSystem));
        SetUseCardBehaviour(new FormulaAttackSelected—haracterBehaviour(abilityData.damage, battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

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

        float speed = Mathf.Ceil(character.MaxSpeed / 2);
        character.MaxSpeed = speed;

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
        character.MaxSpeed = character.Card.speed;
        OnReturnToNormal?.Invoke(this);
        character = null;
    }
}
[Serializable]
public class EternalGuardianCharacterAttackAbilityData: BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float damage;

    public int turnCount;

    [Header("Õ≈ “–Œ√¿“‹")]
    public bool isBuff;
}