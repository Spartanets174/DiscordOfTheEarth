using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAllPlayerUnitsBehaviour : ICardSelectable
{
    private BattleSystem battleSystem;
    public event Action OnSelected;
    public event Action OnCancelSelection;

    private string m_selectCardTipText;
    public string SelectCardTipText
    {
        get
        {
            return m_selectCardTipText;
        }
    }

    public SelectAllPlayerUnitsBehaviour(string text, BattleSystem battleSystem)
    {
        m_selectCardTipText = text;
        this.battleSystem = battleSystem;
    }

    public void SelectCard()
    {
        if (battleSystem.State is PlayerTurn)
        {
            battleSystem.EnemyController.SetEnemiesStates(false,false);

            battleSystem.EnemyController.SetStaticEnemiesState(false);

            battleSystem.PlayerController.SetPlayerStates(true,true);
        }
       
        OnSelected?.Invoke();
    }

    public void CancelSelection()
    {
        if (battleSystem.State is PlayerTurn)
        {
            battleSystem.EnemyController.SetEnemiesStates(true,false);

            battleSystem.EnemyController.SetStaticEnemiesState(true);


            battleSystem.PlayerController.SetPlayerStates(true, false);
        }
        OnCancelSelection?.Invoke();
    }
}
