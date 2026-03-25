using System;
using UnityEngine;

[Serializable]
public class ForestFriendsCardSupportAbility : BaseSupport—ardAbility
{
    private ForestFriendsCardSupportAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (ForestFriendsCardSupportAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("»ÒÔÓÎ¸ÁÛÈÚÂ Í‡ÚÛ"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            battleSystem.FieldController.InvokeActionOnField((x) =>
            {
                if (x.IsSwamp)
                {
                    x.TransitionCostEnemy += abilityData.transitionCost;
                }
            });
        }
        else
        {
            battleSystem.FieldController.InvokeActionOnField((x) =>
            {
                if (x.IsSwamp)
                {
                    x.TransitionCostPlayer += abilityData.transitionCost;
                }
            });
        }

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(null);
    }
}
[Serializable]
public class ForestFriendsCardSupportAbilityData : BaseSupport—ardAbilityData
{
    public int transitionCost;
}