using System;
using UnityEngine;

[Serializable]
public class MaidenOnUnicornCharacterBuffBehaviour : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private Character character;
    private MaidenOnUnicornCharacterBuffBehaviourData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (MaidenOnUnicornCharacterBuffBehaviourData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectAbilityText, battleSystem));
        SetSelectCharacterBehaviour(new EmptySelectCharacterBehaviour(""));
        SetUseCardBehaviour(new EmptyUseAbilityBehaviour());

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
    }


    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += SelectCharacter;
            }
        }

    }


    private void OnSelectCharacter()
    {
        if (battleSystem.State is PlayerTurn)
        {
            character = battleSystem.PlayerController.CurrentPlayerCharacter;
        }
        else
        {
            character = battleSystem.EnemyController.CurrentEnemyCharacter;
        }

        character.PhysAttack+= abilityData.buffAttackAmount;
        character.MagAttack += abilityData.buffAttackAmount;
        character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);


        UseCard(character.gameObject);
    }

    private void OnCardUse()
    {
        OnCancelSelection();
    }

    private void OnCancelSelection()
    {
        battleSystem.PlayerController.SetPlayerStates(true, false, x =>
        {
            x.OnClick -= SelectCharacter;
        });
    }

    public void ReturnToNormal()
    {
        character.PhysAttack-= abilityData.buffAttackAmount;
        character.MagAttack-= abilityData.buffAttackAmount;
        OnReturnToNormal?.Invoke(this);
        character = null;
    }
}
[Serializable]
public class MaidenOnUnicornCharacterBuffBehaviourData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float buffAttackAmount;

    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}