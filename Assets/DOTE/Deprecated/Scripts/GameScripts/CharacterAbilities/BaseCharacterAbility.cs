using System;
using UnityEngine;

[Serializable]
public abstract class BaseCharacterAbility: MonoBehaviour
{
    private Enums.TypeOfAbility m_typeOfAbility;
    public Enums.TypeOfAbility TypeOfAbility=> m_typeOfAbility;

    protected BattleSystem battleSystem;

    protected ICardSelectable m_cardSelectBehaviour;
    public ICardSelectable CardSelectBehaviour => m_cardSelectBehaviour;

    protected ICardSelectable m_cardSecondSelectBehaviour;
    public ICardSelectable CardSecondSelectBehaviour => m_cardSecondSelectBehaviour;

    protected ICardSelectable m_cardThirdSelectBehaviour;
    public ICardSelectable CardThirdSelectBehaviour => m_cardThirdSelectBehaviour;

    protected ICardUsable m_useCardBehaviour;
    public ICardUsable UseCardBehaviour => m_useCardBehaviour;

    protected ICharacterSelectable m_selectCharacterBehaviour;
    public ICharacterSelectable SelectCharacterBehaviour => m_selectCharacterBehaviour;

    protected Character abilityOwner;

    public event Action<ICardSelectable> OnCardAbilitySelected;
    public event Action<ICardSelectable> OnSecondCardAbilitySelected;
    public event Action<ICardSelectable> OnThirdCardAbilitySelected;
    public event Action<ICharacterSelectable> OnCardAbilityCharacterSelected;
    public event Action<BaseCharacterAbility> OnCardAbilityUsed;

    public event Action<BaseCharacterAbility> OnUsingCancel;

    protected void SetCardSelectBehaviour(ICardSelectable behaviour)
    {
        m_cardSelectBehaviour = behaviour;
    }
    protected void SetSecondCardSelectBehaviour(ICardSelectable behaviour)
    {
        m_cardSecondSelectBehaviour = behaviour;
    }

    protected void SetThirdCardSelectBehaviour(ICardSelectable behaviour)
    {
        m_cardThirdSelectBehaviour = behaviour;
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

        OnCardAbilitySelected?.Invoke(m_cardSelectBehaviour);
    }

    public virtual void SelectSecondCard()
    {
        if (m_cardSecondSelectBehaviour != null)
        {
            m_cardSecondSelectBehaviour.SelectCard();
        }

        OnSecondCardAbilitySelected?.Invoke(m_cardSecondSelectBehaviour);
    }

    public virtual void SelectThirdCard()
    {
        if (m_cardThirdSelectBehaviour != null)
        {
            m_cardThirdSelectBehaviour.SelectCard();
        }

        OnThirdCardAbilitySelected?.Invoke(m_cardThirdSelectBehaviour);
    }

    public virtual void SelectCharacter(GameObject gameObject)
    {
        if (m_selectCharacterBehaviour != null)
        {
            m_selectCharacterBehaviour.SelectCharacter(gameObject);
        }
        OnCardAbilityCharacterSelected?.Invoke(m_selectCharacterBehaviour);
    }
    public virtual void UseCard(GameObject gameObject)
    {
        if (m_useCardBehaviour != null)
        {
            m_useCardBehaviour.UseAbility(gameObject);
        }
        OnCardAbilityUsed?.Invoke(this);
    }

    public virtual void CancelUsingCard()
    {
        if (m_cardSelectBehaviour != null)
        {
            m_cardSelectBehaviour.CancelSelection();
        }
        OnUsingCancel?.Invoke(this);
    }

    public void SetAbilityType(Enums.TypeOfAbility typeOfAbility)
    {
        m_typeOfAbility = typeOfAbility;
    }
    public abstract void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData);
}
