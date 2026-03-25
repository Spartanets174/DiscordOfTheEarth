using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SecretKnowledgesCardSupportAbility : BaseSupportÑardAbility
{
    private SecretKnowledgesCardSupportAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupportÑardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (SecretKnowledgesCardSupportAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Èñïîëüçóéòå êàðòó"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var character in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                character.UseAbilityCost = abilityData.pointsOfUsingAbility;
            }
        }
        else
        {
            foreach (var character in battleSystem.EnemyController.EnemyCharObjects)
            {
                character.UseAbilityCost = abilityData.pointsOfUsingAbility;
            }
        }
        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(null);
    }
}
[Serializable]
public class SecretKnowledgesCardSupportAbilityData : BaseSupportÑardAbilityData
{
    public int pointsOfUsingAbility;
}