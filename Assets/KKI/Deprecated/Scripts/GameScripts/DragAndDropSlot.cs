using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerClick != null)
        {
            eventData.pointerDrag.GetComponent<DragAndDropComponent>().OnDropInvoke();
        }
    }
}
