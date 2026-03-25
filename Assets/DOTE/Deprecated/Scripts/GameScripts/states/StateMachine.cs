using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine: MonoBehaviour
{
    public State State;

    public void SetState(State state)
    {
        State?.OnStateCompletedInvoke();
        State = state;
        StartCoroutine(State.Start());
    }
}
