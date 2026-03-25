using System;
using UnityEngine;

[Serializable]
public class LadyOnPonyCharacterBuffAbility : BaseCharacterAbility
{
    private Character character;

    private LadyOnPonyCharacterBuffAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData baseAbilityData)
    {
        character = null;
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (LadyOnPonyCharacterBuffAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsWithConditionBehaviour(abilityData.selectAbilityText, battleSystem, CanBeBuffed));
        SetUseCardBehaviour(new FormulaAttackSelectedÑharacterBehaviour(abilityData.damage, battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

        m_cardSelectBehaviour.OnSelected += OnSelected;

        m_useCardBehaviour.OnCardUse += OnCardUse;

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
    }



    private bool CanBeBuffed(Character character)
    {
        return character.Health > 1;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += UseCard;
            }
        }
    }

    private void OnCardUse()
    {
        if (battleSystem.State is PlayerTurn)
        {
            character = battleSystem.PlayerController.CurrentPlayerCharacter;
        }
        else
        {
            character = battleSystem.EnemyController.CurrentEnemyCharacter;
        }
        character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        character.MagDefence += abilityData.magDefenceAmount;
        character.PhysDefence += abilityData.physDefenceAmount;
        Uncubscribe();
    }

    private void OnCancelSelection()
    {
        Uncubscribe();
    }

    private void Uncubscribe()
    {
        character = null;
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= UseCard;
        }

    }

}
[Serializable]
public class LadyOnPonyCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float damage;

    public float physDefenceAmount;

    public float magDefenceAmount;
}