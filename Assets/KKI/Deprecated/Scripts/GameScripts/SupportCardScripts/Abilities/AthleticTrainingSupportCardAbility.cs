using System;
using UnityEngine;

[Serializable]
public class AthleticTrainingSupportCardAbility : BaseSupportÑardAbility
{
    private AthleticTrainingSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupportÑardAbilityData baseAbilityData)
    {
       this.battleSystem = battleSystem;
        abilityData = (AthleticTrainingSupportCardAbilityData)baseAbilityData;

        SetCardSelectBehaviour(new EmptySelectBehaviour("Èñïîëüçóéòå êàğòó"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        battleSystem.PointsOfAction.Value += abilityData.pointsOfAction;
        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(null);
    }
}
[Serializable]
public class AthleticTrainingSupportCardAbilityData : BaseSupportÑardAbilityData
{
    public int pointsOfAction;
}