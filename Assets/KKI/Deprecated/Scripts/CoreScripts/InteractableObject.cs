using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool m_isEnabled;
    public bool IsEnabled
    {
        get => m_isEnabled;
        set
        {
            m_isEnabled = value;
            OnEnableChanged?.Invoke(m_isEnabled);
        }
    }

    public event Action<bool> OnEnableChanged;
    public event Action<GameObject> OnClick;
    public event Action<GameObject> OnHoverEnter;
    public event Action<GameObject> OnHoverExit;
    public event Action<GameObject> OnHover;
    protected virtual void OnMouseEnter()
    {
        if (IsEnabled)
        {
            OnHoverEnterInvoke();
        }
    }
    protected virtual void OnMouseExit()
    {
        if (IsEnabled)
        {
            OnHoverExitInvoke();
        }
    }

    protected virtual void OnMouseDown()
    {
        if (IsEnabled)
        {
            OnClickInvoke();
        }
    }

    protected virtual void OnMouseOver()
    {
        if (IsEnabled)
        {
            OnHoverInvoke();
        }
    }

    public void OnHoverEnterInvoke()
    {
        OnHoverEnter?.Invoke(gameObject);
    }

    public void OnHoverExitInvoke()
    {
        OnHoverExit?.Invoke(gameObject);

    }

    public void OnClickInvoke()
    {
        OnClick?.Invoke(this.gameObject);
    }

    public void OnHoverInvoke()
    {
        OnHover?.Invoke(gameObject);
    }
}