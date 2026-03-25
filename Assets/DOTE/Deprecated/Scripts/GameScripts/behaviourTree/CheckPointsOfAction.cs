using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointsOfAction : ConditionTask
{
    public BBParameter<BattleSystem> battleSystem;
    protected override bool OnCheck()
    {        
        if (battleSystem.value.PointsOfAction.Value == 0)
        {
            battleSystem.value.SetPlayerTurn();
            return false;
        }
        else
        {
            return true;
        }
    }
}