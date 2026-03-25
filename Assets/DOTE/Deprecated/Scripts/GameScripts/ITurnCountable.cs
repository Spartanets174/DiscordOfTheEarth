using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnCountable 
{
    public bool IsBuff { get; }
    public int TurnCount { get; set; }

    public event Action<ITurnCountable> OnReturnToNormal;
    public void ReturnToNormal();
}
