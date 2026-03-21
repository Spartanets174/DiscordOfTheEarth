using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FormulaAttackSelectedСharacterBehaviour : ICardUsable
{
    public float damage;
    private BattleSystem battleSystem;
    private string abilityName;
    private Character owner;

    public Character attackedCharacter;
    public FormulaAttackSelectedСharacterBehaviour(float damage, BattleSystem battleSystem, Character character, string abilityName)
    {
        this.damage = damage;
        this.battleSystem = battleSystem;
        this.abilityName = abilityName;
        this.owner = character;
    }

    public event Action OnCardUse;
    public void UseAbility(GameObject gameObject)
    {
        Character character = gameObject.GetComponent<Character>();
        if (character is KostilEnemy kostilEnemy)
        {
            character = kostilEnemy.WallEnemyCharacter;
        }

        attackedCharacter = character;
        bool isDeath = character.Damage(owner, abilityName, damage);

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