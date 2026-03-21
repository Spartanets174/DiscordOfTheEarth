using HighlightPlus;
using UnityEngine;

[RequireComponent(typeof(HighlightEffect))]
public class OutlineInteractableObject : InteractableObject
{
    private HighlightEffect highlightEffect;

    private Collider m_collider;
    public Collider Collider => m_collider;
    // Start is called before the first frame update
   protected virtual void Awake()
    {
        highlightEffect = GetComponent<HighlightEffect>();

        m_collider = GetComponent<Collider>();

        OnHoverEnter += EnableOutline;
        OnHoverExit += DisableOutline;
        OnEnableChanged += x => { if (!x) DisableOutline(null); };
    }

    private void OnDestroy()
    {
        OnHoverEnter -= EnableOutline;
        OnHoverExit -= DisableOutline;
    }

    protected void SetOutlineState(bool state)
    {
        highlightEffect.highlighted = state;
    }

    protected void EnableOutline(GameObject gameObject)
    {
        SetOutlineState(true);
    }
    protected void DisableOutline(GameObject gameObject)
    {
        SetOutlineState(false);
    }
}
