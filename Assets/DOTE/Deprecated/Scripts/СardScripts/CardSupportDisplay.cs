
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSupportDisplay : OutlineClicableUI
{
    private CardSupport m_cardSupport;
    public CardSupport CardSupport => m_cardSupport;


    public TextMeshProUGUI cardSupportName;
    public Image background;
    public TextMeshProUGUI ability;
    public TextMeshProUGUI rarity;
    public Image image;

    public void SetValues(CardSupport cardSupport, Sprite rarityBackground)
    {
        if (cardSupport == null) return; 
        IsEnabled = true;
        m_cardSupport = cardSupport;
        cardSupportName.text = m_cardSupport.cardName;
        image.sprite = m_cardSupport.image;
        ability.text = m_cardSupport.abilityText;
        rarity.text = m_cardSupport.rarity.ToString();
        rarity.color = Color.white;
        background.sprite = rarityBackground;
    }
}
