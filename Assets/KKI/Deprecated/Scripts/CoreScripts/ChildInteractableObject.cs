using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract class ChildInteractableObject : MonoBehaviour
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

    protected CompositeDisposable disposables = new();

    protected Collider m_collider;
    public Collider Collider => m_collider;

    public event Action<bool> OnEnableChanged;
    public event Action<GameObject> OnClick;
    public event Action<GameObject> OnHoverEnter;
    public event Action<GameObject> OnHoverExit;
    public event Action<GameObject> OnHover;
    protected virtual void Awake()
    {       
        m_collider = GetComponent<Collider>();
        if (m_collider == null)
        {
            m_collider = GetComponentInChildren<Collider>();
        }
       
        m_collider.OnMouseEnterAsObservable().Where(x => IsEnabled).Subscribe(
            x => OnHoverEnterInvoke()
            ).AddTo(disposables);       

        m_collider.OnMouseDownAsObservable().Where(x => IsEnabled).Subscribe(
            x => OnClickInvoke()
            ).AddTo(disposables);

        m_collider.OnMouseOverAsObservable().Where(x => IsEnabled).Subscribe(
            x => OnHoverInvoke()
            ).AddTo(disposables);
        SubscribeOnMouseExit();
    }

    protected virtual void SubscribeOnMouseExit()
    {
        m_collider.OnMouseExitAsObservable().Where(x => IsEnabled).Subscribe(
            x => OnHoverExitInvoke()
            ).AddTo(disposables);
    }
    private void OnDestroy()
    {
        disposables.Dispose();
        disposables.Clear();
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