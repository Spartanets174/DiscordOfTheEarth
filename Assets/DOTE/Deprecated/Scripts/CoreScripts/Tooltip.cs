using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tooltipText;

    public void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;
    }


    private void Update()
    {
        /*RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector2 localPoint);*/
        transform.position = Input.mousePosition;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
