using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public abstract class State 
{
    protected BattleSystem BattleSystem;

    protected CompositeDisposable disposables = new();

    public bool IsSupportCardUsed = false;

    public event Action<GameCharacterCardDisplay> OnCharacterChosen;
    public event Func<GameCharacterCardDisplay> OnCharacterMoved;

    public event Action OnStateStarted;
    public event Action<State> OnStateCompleted;
    public State(BattleSystem battleSystem)
    {
        BattleSystem = battleSystem;
    }
    protected void OnCharacterChosenInvoke(GameCharacterCardDisplay gameCharacterCardDisplay)
    {
        OnCharacterChosen?.Invoke(gameCharacterCardDisplay);
    }

    protected GameCharacterCardDisplay OnCharacterMovedInvoke()
    {
        return OnCharacterMoved?.Invoke();
    }

    public void OnStateStartedInvoke()
    {
        OnStateStarted?.Invoke();
    }

    public void OnStateCompletedInvoke()
    {
        OnStateCompleted?.Invoke(this);
    }


    public virtual IEnumerator Start()
    {
        yield break;
    }
    public virtual IEnumerator ChooseCharacter(GameObject character)
    {
        yield break;
    }
    public virtual IEnumerator Move(GameObject cell)
    {
        yield break;
    }
    public virtual IEnumerator Attack(GameObject target)
    {
        yield break;
    }
    public virtual IEnumerator UseAttackAbility(GameObject gameObject)
    {
        yield break;
    }
    public virtual IEnumerator UseDefensiveAbility(GameObject gameObject)
    {
        yield break;
    }
    public virtual IEnumerator UseBuffAbility(GameObject gameObject)
    {
        yield break;
    }
    public virtual IEnumerator UseSupportCard(GameObject cardSupport)
    {
        yield break;
    }
    public virtual IEnumerator UseItem()
    {
        yield break;
    }
}
