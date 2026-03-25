using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FormulaAttackSelectedСharactersBehaviour : ICardUsable
{
    public List<Character> characters = new List<Character>();
    public float damageMultiplier;
    private float damage;
    private BattleSystem battleSystem;
    private string abilityName;
    private Character owner;
    public FormulaAttackSelectedСharactersBehaviour(float damage, BattleSystem battleSystem, Character character, string abilityName, float damageMultiplier = 0)
    {
        this.damage = damage;
        this.battleSystem = battleSystem;
        this.abilityName = abilityName;
        this.owner = character;
        this.damageMultiplier = damageMultiplier;
    }

    public event Action OnCardUse;
    public void UseAbility(GameObject gameObject)
    {
        List<Character> tempCharacters = new(characters);
        foreach (var tempCharacter in tempCharacters)
        {
            Character character = tempCharacter;
            if (character is KostilEnemy kostilEnemy)
            {
                character = kostilEnemy.WallEnemyCharacter;
            }

            float tempDamage = characters.IndexOf(tempCharacter) > 0 ? damage * (1 + damageMultiplier) : damage;

            bool isDeath = character.Damage(owner, abilityName, tempDamage);

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

            OnCardUse?.Invoke();
        }       
    }
}