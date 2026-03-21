using System;
using UnityEngine;

[Serializable]
public abstract class BaseSupportСardAbility: MonoBehaviour
{
    protected BattleSystem battleSystem;

    protected ICardSelectable m_cardSelectBehaviour;
    public ICardSelectable CardSelectBehaviour => m_cardSelectBehaviour;

    protected ICardSelectable m_cardSecondSelectBehaviour;
    public ICardSelectable CardSecondSelectBehaviour => m_cardSecondSelectBehaviour;

    protected ICardUsable m_useCardBehaviour;
    public ICardUsable UseCardBehaviour => m_useCardBehaviour;

    protected ICharacterSelectable m_selectCharacterBehaviour;
    public ICharacterSelectable SelectCharacterBehaviour => m_selectCharacterBehaviour;

    public event Action<ICardSelectable> OnSupportCardAbilitySelected;
    public event Action<ICardSelectable> OnSecondSupportCardAbilitySelected;
    public event Action<ICharacterSelectable> OnSupportCardAbilityCharacterSelected;
    public event Action<ICardUsable> OnSupportCardAbilityUsed;

    public event Action<BaseSupportСardAbility> OnUsingCancel;

    protected void SetCardSelectBehaviour(ICardSelectable behaviour)
    {
        m_cardSelectBehaviour = behaviour;
    }
    protected void SetSecondCardSelectBehaviour(ICardSelectable behaviour)
    {
        m_cardSecondSelectBehaviour = behaviour;
    }
    protected void SetUseCardBehaviour(ICardUsable behaviour)
    {
        m_useCardBehaviour = behaviour;
    }
    protected void SetSelectCharacterBehaviour(ICharacterSelectable behaviour)
    {
        m_selectCharacterBehaviour = behaviour;
    }

    public virtual void SelectCard()
    {
        if (m_cardSelectBehaviour != null)
        {
            m_cardSelectBehaviour.SelectCard();
        }

        OnSupportCardAbilitySelected?.Invoke(m_cardSelectBehaviour);
    }

    public virtual void SelectSecondCard()
    {
        if (m_cardSecondSelectBehaviour != null)
        {
            m_cardSecondSelectBehaviour.SelectCard();
        }

        OnSecondSupportCardAbilitySelected?.Invoke(m_cardSecondSelectBehaviour);
    }

    public virtual void UseCard(GameObject gameObject)
    {
        if (m_useCardBehaviour != null)
        {
            m_useCardBehaviour.UseAbility(gameObject);
        }
        OnSupportCardAbilityUsed?.Invoke(m_useCardBehaviour);
    }

    public virtual void SelectCharacter(GameObject gameObject)
    {
        if (m_selectCharacterBehaviour != null)
        {
            m_selectCharacterBehaviour.SelectCharacter(gameObject);
        }
        OnSupportCardAbilityCharacterSelected?.Invoke(m_selectCharacterBehaviour);
    }

    public virtual void CancelUsingCard()
    {
        if (m_cardSelectBehaviour != null)
        {
            m_cardSelectBehaviour.CancelSelection();
        }
        OnUsingCancel?.Invoke(this);
    }

    public abstract void Init(BattleSystem battleSystem, BaseSupportСardAbilityData baseSupportСardAbilityData);
}
