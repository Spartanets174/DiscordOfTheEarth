using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OldMasterCharacterBuffAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    private List<Character> characterList = new List<Character>();
    private OldMasterCharacterBuffbilityData abilityData;

    public event Action<ITurnCountable> OnReturnToNormal;

    private float increaseAmount;
    private float healAmount;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (OldMasterCharacterBuffbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {

        SetCharacters();
        foreach (var character in characterList)
        {
            increaseAmount = (character.MaxHealth * (1 + abilityData.healthIcnreaseAmount)) - character.MaxHealth;
            character.MaxHealth = character.MaxHealth * (1 + abilityData.healthIcnreaseAmount);

            if (character.Health == character.MaxHealth)
            {
                character.Heal(character.MaxHealth, abilityOwner.CharacterName);
                healAmount = increaseAmount;
            }
            else
            {
                healAmount = character.Health * abilityData.healthIcnreaseAmount;
                character.Heal(healAmount, abilityOwner.CharacterName);
            }
        }



        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);
    }

    private void SetCharacters()
    {
        while (characterList.Count < 2)
        {
            Character character;
            if (battleSystem.State is PlayerTurn)
            {
                character = battleSystem.PlayerController.PlayerCharactersObjects[UnityEngine.Random.Range(0, battleSystem.PlayerController.PlayerCharactersObjects.Count)];
                if (!characterList.Contains(character))
                {
                    characterList.Add(character);
                }
            }
            else
            {
                character = battleSystem.EnemyController.EnemyCharObjects[UnityEngine.Random.Range(0, battleSystem.EnemyController.EnemyCharObjects.Count)];
                if (!characterList.Contains(character))
                {
                    characterList.Add(character);
                }
            }
        }
    }

    public void ReturnToNormal()
    {
        foreach (var character in characterList)
        {
            character.MaxHealth = character.Card.health;
            if (character.Health == character.MaxHealth)
            {
                character.Damage(increaseAmount, "\"Мы одна кровь\"");
            }
            else
            {
                character.Damage(healAmount, "\"Мы одна кровь\"");
            }
        }

        characterList.Clear();

        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class OldMasterCharacterBuffbilityData : BaseCharacterAbilityData
{
    public float healthIcnreaseAmount;

    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}