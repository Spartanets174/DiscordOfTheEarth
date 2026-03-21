using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CleansingSecondSupportCardAbility : BaseSupportÑardAbility
{
    private CleansingSecondSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupportÑardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (CleansingSecondSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Èñïîëüçóéòå êàðòó"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            battleSystem.PlayerController.RemoveDebuffsAllPlayerCharacters();
            foreach (var character in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                character.InstantiateEffectOnCharacter(abilityData.effect);
            }
        }
        else
        {
            battleSystem.EnemyController.RemoveDebuffsAllEnemyCharacters();
            foreach (var character in battleSystem.EnemyController.EnemyCharObjects)
            {
                character.InstantiateEffectOnCharacter(abilityData.effect);
            }

        }


        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(null);
    }

}
[Serializable]
public class CleansingSecondSupportCardAbilityData : BaseSupportÑardAbilityData
{
    
}