using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackAllCharactersInAreaBehaviour : ICardUsable
{
    public List<Cell> cellsToAttack;
    public List<Character> attackedCharacters;
    private float damage;
    private BattleSystem battleSystem;
    private string abilityName;
    public AttackAllCharactersInAreaBehaviour(float damage, BattleSystem battleSystem, string abilityName)
    {
        cellsToAttack = new();
        attackedCharacters = new();
        this.damage = damage;
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
            bool isDeath = character.Damage(damage, abilityName);

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
                battleSystem.gameLogCurrentText.Value = $"{characterType} персонаж <color=#{ColorUtility.ToHtmlStringRGB(characterColor)}>{character.CharacterName}</color> погибает от эффекта карты \"{abilityName}\"";
                GameObject.Destroy(character.gameObject);
            }
        }
        OnCardUse?.Invoke();
    }

}
