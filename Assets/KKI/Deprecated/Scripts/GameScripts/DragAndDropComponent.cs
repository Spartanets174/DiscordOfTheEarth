using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropComponent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector3 m_startPos;
    public Vector3 StartPos
    {
        get => m_startPos;
        set => m_startPos = value;
    }

    private bool m_isAllowedToDrag;
    public bool IsAllowedToDrag
    {
        get => m_isAllowedToDrag;
        set => m_isAllowedToDrag=value;
    }

    public event Action<GameObject> OnDropEvent;
    public event Action<GameObject> OnBeginDragEvent;
    public event Action<GameObject> OnEndDragEvent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindObjectsOfType<Canvas>().Where(x => x.renderMode == RenderMode.ScreenSpaceOverlay).ToList()[0] ;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_isAllowedToDrag)
        {
            canvasGroup.blocksRaycasts = false;
            OnBeginDragEvent?.Invoke(this.gameObject);
        }       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_isAllowedToDrag)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_isAllowedToDrag)
        {
            canvasGroup.blocksRaycasts = true;
            OnEndDragEvent?.Invoke(this.gameObject);
        }      
    }

    public void OnDropInvoke()
    {
        canvasGroup.blocksRaycasts = true;
        OnDropEvent?.Invoke(gameObject);
    }
}
