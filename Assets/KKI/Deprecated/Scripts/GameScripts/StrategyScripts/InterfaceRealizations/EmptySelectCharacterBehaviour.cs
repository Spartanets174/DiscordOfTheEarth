using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySelectCharacterBehaviour : ICharacterSelectable
{
    private string m_selectCharacterTipText;
    public string SelectCharacterTipText => m_selectCharacterTipText;

    public event Action OnSelectCharacter;

    public EmptySelectCharacterBehaviour(string text)
    {
        m_selectCharacterTipText = text;
    }

    public void SelectCharacter(GameObject gameObject)
    {
        OnSelectCharacter?.Invoke();
    }
}
