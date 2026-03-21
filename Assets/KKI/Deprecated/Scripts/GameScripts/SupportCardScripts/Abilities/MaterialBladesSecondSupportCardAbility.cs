using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialBladesSecondSupportCardAbility : BaseSupportСardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private List<Character> characters = new();
    AttackSelectedСharactersBehaviour attackSelectedСharactersBehaviour;
    private MaterialBladesSecondSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupportСardAbilityData baseAbilityData)
    {
        characters.Clear();
        this.battleSystem = battleSystem;
        abilityData = (MaterialBladesSecondSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectAllEnemyUnitsBehaviour(abilityData.selectCardText, battleSystem));
        SetSecondCardSelectBehaviour(new SelectAllEnemyUnitsBehaviour(abilityData.selectCharacterText, battleSystem));
        SetSelectCharacterBehaviour(new SetCurrentEnemyCharacterBehaviour("", battleSystem));
        SetUseCardBehaviour(new AttackSelectedСharactersBehaviour(abilityData.damage, battleSystem, $"\"{abilityData.supportСardAbilityName}\""));

        attackSelectedСharactersBehaviour = (AttackSelectedСharactersBehaviour)UseCardBehaviour;

        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_cardSecondSelectBehaviour.OnSelected += OnSecondSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
        m_useCardBehaviour.OnCardUse += OnCardUse;

        m_cardSecondSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var enemyCharacter in battleSystem.EnemyController.EnemyCharObjects)
            {
                enemyCharacter.OnClick += SelectSecondCharacterInvoke;
            }

        }
    }

    private void SelectSecondCharacterInvoke(GameObject gameObject)
    {
        characters.Add(gameObject.GetComponent<Character>());

        battleSystem.EnemyController.SetEnemiesChosenState(false, x =>
        {
            x.OnClick -= SelectSecondCharacterInvoke;
        });

        SelectSecondCard();
    }



    private void OnSecondSelected()
    {
        foreach (var enemyCharacter in battleSystem.EnemyController.EnemyCharObjects)
        {
            enemyCharacter.OnClick += SelectCharacter;
        }
    }


    private void OnSelectCharacter()
    {
        if (battleSystem.State is PlayerTurn)
        {
            characters.Add(battleSystem.EnemyController.CurrentEnemyCharacter);
        }
        else
        {
            characters.Add(battleSystem.PlayerController.CurrentPlayerCharacter);
        }

        attackSelectedСharactersBehaviour.charactersToAttack = characters;
        foreach (var character in characters)
        {
            character.IsFreezed = true;
            character.InstantiateEffectOnCharacter(abilityData.effect);
        }
        UseCard(null);
    }

    private void OnCardUse()
    {
        Uncubscribe();
    }

    private void OnCancelSelection()
    {
        characters.Clear();
        Uncubscribe();
    }

    private void Uncubscribe()
    {
        battleSystem.EnemyController.SetEnemiesStates(true, false, x =>
        {
            x.OnClick -= SelectSecondCharacterInvoke;
            x.OnClick -= SelectCharacter;
        });

        battleSystem.PlayerController.SetPlayerStates(true, false);
    }

    public void ReturnToNormal()
    {
        foreach (var character in characters)
        {
            character.IsFreezed = false;
        }
        OnReturnToNormal?.Invoke(this);
    }
}
[Serializable]
public class MaterialBladesSecondSupportCardAbilityData : BaseSupportСardAbilityData
{
    public string selectCardText;
    public string selectCharacterText;

    public float damage;

    public int turnCount;

    [Header("Не трогать!")]
    public bool isBuff;
}