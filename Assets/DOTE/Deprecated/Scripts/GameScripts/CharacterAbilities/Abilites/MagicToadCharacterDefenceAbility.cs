using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagicToadCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{ 
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    private Character character;
    private GameObject effect;
    private MagicToadCharacterDefenceAbilityData abilityData;

    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (MagicToadCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllEnemyUnitsBehaviour(abilityData.selectAbilityText, battleSystem));
        SetSelectCharacterBehaviour(new SetCurrentEnemyCharacterBehaviour("", battleSystem));
        SetUseCardBehaviour(new EmptyUseAbilityBehaviour());

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
        character.IsFreezed = true;
        character.CanDamage = false;
        character.CanUseAbilities = false;
        effect =  character.InstantiateEffectOnCharacter(abilityData.useEffects["freeze"]);

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
        character.IsFreezed = false;
        character.CanDamage = true;
        character.CanUseAbilities = false;
        Destroy(effect);
        OnReturnToNormal?.Invoke(this);
        character = null;
    }
}
[Serializable]
public class MagicToadCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public int turnCount;

    [Header("Не трогать!!!")]
    public bool isBuff;
}