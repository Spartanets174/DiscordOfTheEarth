using System;
using UnityEngine;

public class SetCurrentEnemyCharacterBehaviour : ICharacterSelectable
{
    private string m_selectCharacterTipText;
    public string SelectCharacterTipText => m_selectCharacterTipText;

    private BattleSystem m_battleSystem;
    public event Action OnSelectCharacter;

    public SetCurrentEnemyCharacterBehaviour(string text, BattleSystem battleSystem)
    {
        m_selectCharacterTipText = text;
        m_battleSystem = battleSystem;
    }

    public void SelectCharacter(GameObject gameObject)
    {
        if (m_battleSystem.State is PlayerTurn)
        {
            m_battleSystem.EnemyController.SetCurrentEnemyChosenCharacter(gameObject.GetComponent<EnemyCharacter>());
        }
        else
        {
            m_battleSystem.PlayerController.SetCurrentPlayerChosenCharacter(gameObject);
        }
        
        OnSelectCharacter?.Invoke();
    }
}
