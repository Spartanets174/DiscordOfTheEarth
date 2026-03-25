using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EternalGuardianCharacterBuffAbility : BaseCharacterAbility
{
    private EternalGuardianCharacterBuffAbilityData abilityData;

    private List<Character> characters = new();

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData baseCharacterAbility)
    {
        characters.Clear();
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (EternalGuardianCharacterBuffAbilityData)baseCharacterAbility;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsWithConditionBehaviour(abilityData.selectAbilityText, battleSystem, CanBeHealed));
        SetSecondCardSelectBehaviour(new SelectAllPlayerUnitsWithConditionBehaviour(abilityData.selectCharacterText, battleSystem, CanBeHealed));
        SetSelectCharacterBehaviour(new EmptySelectCharacterBehaviour(""));
        SetUseCardBehaviour(new EmptyUseAbilityBehaviour());

        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_cardSecondSelectBehaviour.OnSelected += OnSecondSelected;

        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
        m_useCardBehaviour.OnCardUse += OnCardUse;

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSecondSelectBehaviour.OnCancelSelection += OnCancelSelection;
    }



    private bool CanBeHealed(Character character)
    {
        return character.Health != character.MaxHealth;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += SelectSecondCharacterInvoke;
            }
        }
    }

    private void SelectSecondCharacterInvoke(GameObject gameObject)
    {
        characters.Add(gameObject.GetComponent<Character>());

        battleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= SelectSecondCharacterInvoke;
        });

        SelectSecondCard();
    }
    private void OnSecondSelected()
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick += SelectCharacter;
        }
    }

    private void OnSelectCharacter()
    {
        if (battleSystem.State is PlayerTurn)
        {
            characters.Add(battleSystem.PlayerController.CurrentPlayerCharacter);
        }
        else
        {
            characters.Add(battleSystem.EnemyController.CurrentEnemyCharacter);
        }

        battleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= SelectCharacter;
        });

        if (characters.Count == 2)
        {
            foreach (var character in characters)
            {
                if (character != null)
                {
                    character.Heal(abilityData.healAmount, abilityOwner.CharacterName);
                }
            }
        }
        UseCard(abilityOwner.gameObject);
    }

    private void OnCardUse()
    {
        Uncubscribe();
    }

    private void OnCancelSelection()
    {
        Uncubscribe();
    }

    private void Uncubscribe()
    {
        characters.Clear();
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= SelectSecondCharacterInvoke;
            playerCharacter.OnClick -= SelectCharacter;
        }

    }

}
[Serializable]
public class EternalGuardianCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public string selectCharacterText;

    public float healAmount;
}