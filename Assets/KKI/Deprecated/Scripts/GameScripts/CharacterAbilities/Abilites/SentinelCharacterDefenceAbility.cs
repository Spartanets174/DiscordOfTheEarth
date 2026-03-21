using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class SentinelCharacterDefenceAbility : BaseCharacterAbility
{
    private SentinelCharacterDefenceAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (SentinelCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectAbilityText, battleSystem));
        SetSelectCharacterBehaviour(new EmptySelectCharacterBehaviour(""));

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
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
            battleSystem.PlayerController.CurrentPlayerCharacter.PhysDefence += abilityData.physDefenceAmount;
            battleSystem.PlayerController.CurrentPlayerCharacter.MagDefence += abilityData.magDefenceAmount;
            battleSystem.PlayerController.CurrentPlayerCharacter.CritChance += abilityData.critChanceAmount;
            battleSystem.PlayerController.CurrentPlayerCharacter.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        }
        else
        {
            battleSystem.EnemyController.CurrentEnemyCharacter.PhysDefence += abilityData.physDefenceAmount;
            battleSystem.EnemyController.CurrentEnemyCharacter.MagDefence += abilityData.magDefenceAmount;
            battleSystem.EnemyController.CurrentEnemyCharacter.CritChance += abilityData.critChanceAmount;
            battleSystem.EnemyController.CurrentEnemyCharacter.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);


        }

        battleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= SelectCharacter;
        });

        UseCard(null);
    }

    private void OnCancelSelection()
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= SelectCharacter;
        }
    }

}
[Serializable]
public class SentinelCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float magDefenceAmount;

    public float physDefenceAmount;

    public float critChanceAmount;
}