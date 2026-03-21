using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class KeenEyeSupportCardAbility : BaseSupportÑardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    private List<EnemyCharacter> enemyCharacters;
    private List<PlayerCharacter> playerCharacters;
    private KeenEyeSupportCardAbilityData abilityData;

    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, BaseSupportÑardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (KeenEyeSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Èñïîëüçóéòå êàğòó"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            playerCharacters = battleSystem.PlayerController.PlayerCharactersObjects.Where(x => x.Class == Enums.Classes.Ëó÷íèê).ToList();
            foreach (var playerCharacter in playerCharacters)
            {
                playerCharacter.CritChance = abilityData.critChance;
                playerCharacter.InstantiateEffectOnCharacter(abilityData.effect);

            }
        }
        else
        {
            enemyCharacters = battleSystem.EnemyController.EnemyCharObjects.Where(x => x.Class == Enums.Classes.Ëó÷íèê).ToList();
            foreach (var enemyCharacter in enemyCharacters)
            {
                enemyCharacter.CritChance = abilityData.critChance;
                enemyCharacter.InstantiateEffectOnCharacter(abilityData.effect);

            }
        }

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(null);
    }

    public void ReturnToNormal()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in playerCharacters)
            {
                playerCharacter.CritChance = playerCharacter.Card.critChance;
            }
        }
        else
        {
            foreach (var enemyCharacter in enemyCharacters)
            {
                enemyCharacter.CritChance = enemyCharacter.Card.critChance;
            }
        }
        OnReturnToNormal?.Invoke(this);
    }
}
[Serializable]
public class KeenEyeSupportCardAbilityData : BaseSupportÑardAbilityData
{
    public  float critChance;

    public int turnCount;

    [Header("Íå òğîãàòü!")]
    public bool isBuff;
}