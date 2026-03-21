using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class ChosenSupportCardDisplay : MonoBehaviour
{
    [SerializeField]
    private Image supportCardImage;
    [SerializeField]
    private Image supportCardRarity;
    [SerializeField]
    private TextMeshProUGUI supportCardRarityText;
    [SerializeField]
    private TextMeshProUGUI supportCardAbility;
    [SerializeField]
    private TextMeshProUGUI supportCardName;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color mythColor;

    [HideInInspector]
    public ReactiveProperty<CardSupport> chosenCardSupport = new();


    private void Start()
    {
        chosenCardSupport.Value = null;
    }

    public void SetSupportCardData(GameObject cardSupportObject)
    {
        CardSupport cardSupport = cardSupportObject.GetComponent<CardSupportDisplay>().CardSupport;
        chosenCardSupport.Value = cardSupport;
        supportCardImage.sprite = cardSupport.image;
        supportCardImage.DOFade(1, 0);
        supportCardRarity.DOFade(1, 0);
        if (cardSupport.rarity==Enums.Rarity.Мифическая)
        {
            supportCardRarity.color = mythColor;
        }
        else
        {
            supportCardRarity.color = normalColor;
        }
        supportCardRarityText.text = cardSupport.rarity.ToString();
        supportCardAbility.text = $"Способность: {cardSupport.abilityText}";
        supportCardName.text = cardSupport.cardName;
    }

    public void SetOnlyData(GameObject cardSupportObject)
    {
        CardSupport cardSupport = cardSupportObject.GetComponent<DeckSupportCardDisplay>().CurrentCardSupport;
        chosenCardSupport.Value = null;
        supportCardImage.sprite = cardSupport.image;
        supportCardImage.DOFade(1, 0);
        supportCardRarity.DOFade(1, 0);
        if (cardSupport.rarity == Enums.Rarity.Мифическая)
        {
            supportCardRarity.color = mythColor;
        }
        else
        {
            supportCardRarity.color = normalColor;
        }
        supportCardRarityText.text = cardSupport.rarity.ToString();
        supportCardAbility.text = $"Способность: {cardSupport.abilityText}";
        supportCardName.text = cardSupport.cardName;
    }
}
