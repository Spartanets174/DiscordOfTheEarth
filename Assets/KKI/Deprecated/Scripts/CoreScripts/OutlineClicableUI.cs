using Nobi.UiRoundedCorners;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UI.Outline))]
public class OutlineClicableUI : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField]
    private UnityEngine.UI.Outline outline;
    [SerializeField]
    protected GameObject blocker;



    public event Action<GameObject> OnDoubleClick;
    public event Action<GameObject> OnClick;
    public event Action OnHoverExit;
    public event Action OnHoverEnter;

    private bool m_isEnabled;
    private float thresholdDoubleClick = 0.2f;
    private DateTime lastClick;

    public bool IsEnabled
    {
        get
        {
            return m_isEnabled;
        }
        set 
        {
            m_isEnabled = value;
            SetBlockerState(!m_isEnabled);
        }
    }

    public void SetBlockerState(bool state)
    {
        if (blocker != null)
        {
            blocker.SetActive(state);
        }
    }
    
    private void Start()
    {
        outline.enabled = false;
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2, -2);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_isEnabled)
        {
            outline.enabled = false;
            OnHoverExit?.Invoke();
        }      
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (m_isEnabled)
        {
            OnClick?.Invoke(eventData.pointerClick);
            if ((DateTime.Now - lastClick).TotalSeconds <= thresholdDoubleClick)
            {
                OnDoubleClick?.Invoke(eventData.pointerClick);
            }
            else
            {
                lastClick = DateTime.Now;
            }
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (m_isEnabled)
        {
            outline.enabled = true;
            OnHoverEnter?.Invoke();
        }
    }
}
