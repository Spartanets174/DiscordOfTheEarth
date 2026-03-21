using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyUseAbilityBehaviour: ICardUsable
{
    public event Action OnCardUse;

    public void UseAbility(GameObject gameObject)
    {
        OnCardUse?.Invoke();
    }
}
