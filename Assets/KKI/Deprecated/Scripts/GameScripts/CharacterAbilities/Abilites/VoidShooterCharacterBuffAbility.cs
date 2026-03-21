using System;
using UnityEngine;

[Serializable]
public class VoidShooterCharacterBuffAbility : BaseCharacterAbility
{
    private Character character;
    private VoidShooterCharacterBuffAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character abilityOwner, BaseCharacterAbilityData baseCharacterAbility)
    {
        this.abilityOwner = abilityOwner;
        this.battleSystem = battleSystem;
        abilityData = (VoidShooterCharacterBuffAbilityData)baseCharacterAbility;
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

        float tempChance = UnityEngine.Random.Range(0f, 1f);
        Character tempCharacter = battleSystem.PlayerController.PlayerCharactersObjects[UnityEngine.Random.Range(0, battleSystem.PlayerController.PlayerCharactersObjects.Count)];
        if (tempChance <= abilityData.chance)
        {
            character.PhysDefence -= abilityData.amount;
            tempCharacter.PhysDefence += abilityData.amount;
        }
        else
        {
            character.MagDefence -= abilityData.amount;
            tempCharacter.MagDefence += abilityData.amount;

        }
        tempCharacter.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

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
}
[Serializable]
public class VoidShooterCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float chance;

    public float amount;

}