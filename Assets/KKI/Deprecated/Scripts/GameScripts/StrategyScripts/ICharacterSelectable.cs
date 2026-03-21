using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterSelectable 
{
    public string SelectCharacterTipText { get; }
    public event Action OnSelectCharacter;
    public void SelectCharacter(GameObject gameObject);
}
