using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectAllPlayerUnitsWithConditionBehaviour : ICardSelectable
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

    private Func<Character,bool> conditionAction;
    public SelectAllPlayerUnitsWithConditionBehaviour(string text, BattleSystem battleSystem, Func<Character, bool> conditionAction)
    {
        m_selectCardTipText = text;
        this.battleSystem = battleSystem;
        this.conditionAction = conditionAction;
    }

    public void SelectCard()
    {
        if (battleSystem.State is PlayerTurn)
        {
            battleSystem.EnemyController.SetEnemiesStates(false, false);
           
            battleSystem.EnemyController.SetStaticEnemiesState(false);

            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                if (conditionAction.Invoke(playerCharacter))
                {
                    playerCharacter.IsEnabled = true;
                    playerCharacter.IsChosen = true;
                }
                else
                {
                    playerCharacter.IsEnabled = false;
                    playerCharacter.IsChosen = false;
                }
            }        
        }

        OnSelected?.Invoke();
    }

    public void CancelSelection()
    {
        if (battleSystem.State is PlayerTurn)
        {
            battleSystem.EnemyController.SetEnemiesStates(true, false);

            battleSystem.EnemyController.SetStaticEnemiesState(true);


            battleSystem.PlayerController.SetPlayerStates(true, false);
        }
        OnCancelSelection?.Invoke();
    }
}

