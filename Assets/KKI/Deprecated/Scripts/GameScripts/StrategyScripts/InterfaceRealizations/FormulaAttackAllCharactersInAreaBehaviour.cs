using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FormulaAttackEnemyCharactersInAreaBehaviour : ICardUsable
{
    public List<Cell> cellsToAttack;
    public List<Character> attackedCharacters;
    private float damage;
    private BattleSystem battleSystem;
    private string abilityName;
    private Character chosenCharacter;
    public FormulaAttackEnemyCharactersInAreaBehaviour(float damage, BattleSystem battleSystem,Character chosenCharacter, string abilityName)
    {
        cellsToAttack = new();
        attackedCharacters = new();
        this.damage = damage;
        this.chosenCharacter = chosenCharacter;
        this.battleSystem = battleSystem;
        this.abilityName = abilityName;
    }

    public event Action OnCardUse;
    public void UseAbility(GameObject gameObject)
    {
        foreach (var cell in cellsToAttack)
        {
            Character character = cell.GetComponentInChildren<Character>();
            if (character == null) continue;
            if (character is KostilEnemy kostilEnemy)
            {
                character = kostilEnemy.WallEnemyCharacter;
            }

            attackedCharacters.Add(character);
            bool isDeath = character.Damage(chosenCharacter, abilityName, damage);

            if (isDeath)
            {
                string characterType = "";
                Color characterColor;
                if (character is PlayerCharacter)
                {
                    characterType = "Союзный";
                    characterColor = battleSystem.playerTextColor;
                }
                else
                {
                    characterType = "Вражеский";
                    characterColor = battleSystem.enemyTextColor;

                }
                battleSystem.gameLogCurrentText.Value = $"{characterType} персонаж <color=#{characterColor.ToHexString()}>{character.CharacterName}</color> погибает от эффекта карты \"{abilityName}\"";
                GameObject.Destroy(character.gameObject);
            }
        }
        OnCardUse?.Invoke();
    }

}
