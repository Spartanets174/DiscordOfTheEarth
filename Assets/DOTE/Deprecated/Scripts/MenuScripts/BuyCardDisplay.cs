
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyCardDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerDataController playerManager;

    [SerializeField]
    private Image cardSprite;
    [SerializeField]
    private TextMeshProUGUI cardAbilities;
    [SerializeField]
    private TextMeshProUGUI cardPrice;
    [SerializeField]
    private TextMeshProUGUI moneyOfPlayer;
    [SerializeField]
    private TextMeshProUGUI NotEnoughtCaption;
    [SerializeField]
    private TextMeshProUGUI cardName;


    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button m_buyButton;
    public Button BuyButton => m_buyButton;

    [SerializeField]
    private Color attackColor;
    [SerializeField]
    private Color defenceColor;
    [SerializeField]
    private Color buffColor;
    [SerializeField]
    private Color passiveColor;

    private Card m_chosenCard;
    public Card ChosenCard => m_chosenCard;
    DG.Tweening.Sequence currentSequence;


    private void Start()
    {
        m_buyButton.onClick.AddListener(UpdateMoneyText);
        closeButton.onClick.AddListener(KillSequence);
    }

    private void OnDestroy()
    {
        m_buyButton.onClick.RemoveListener(UpdateMoneyText);
        closeButton.onClick.RemoveListener(KillSequence);
    }
    private void UpdateMoneyText()
    {
        moneyOfPlayer.text = playerManager.Money.ToString() + "$";
    }

    private void KillSequence()
    {
        NotEnoughtCaption.gameObject.SetActive(false);
    }

    public IEnumerator TurnOffNotEnoughtCaption()
    {
        NotEnoughtCaption.gameObject.SetActive(true);
        NotEnoughtCaption.text = $"Не хватает: {m_chosenCard.Price - playerManager.Money}$";
        yield return new WaitForSecondsRealtime(2);
        currentSequence = DOTween.Sequence();
        currentSequence.id = 1;
        currentSequence.Append(NotEnoughtCaption.DOFade(0,2f))
        .OnComplete(() => {
            NotEnoughtCaption.color = Color.red;
            NotEnoughtCaption.gameObject.SetActive(false);
            currentSequence.Kill();
        });
        currentSequence.Play();
        
    }

    public void SetCardInfo(Card card)
    {
        cardName.text = card.cardName;
        m_chosenCard = card;
        cardSprite.sprite = card.image;
        cardPrice.text = "Цена "+card.Price.ToString()+"$";
        moneyOfPlayer.text = playerManager.Money.ToString() + "$";

        if (card is CharacterCard)
        {
            CharacterCard characterCard = (CharacterCard)card;
            cardAbilities.text = $"<color=#{attackColor.ToHexString()}>Атакующая способность</color>: {characterCard.attackAbility}" + "\n" + "\n" +
                    $"<color=#{defenceColor.ToHexString()}>Защитная способность</color>: {characterCard.defenceAbility}" + "\n" + "\n" +
                    $"<color=#{buffColor.ToHexString()}>Усиливающая способность</color>: {characterCard.buffAbility}" + "\n" + "\n" +
                    $"<color=#{passiveColor.ToHexString()}>Пассивная способность</color>: {characterCard.passiveAbility}";

        }
        if (card is CardSupport)
        {
            CardSupport cardSupport = (CardSupport)card;
            cardAbilities.text = $"Способность: {cardSupport.abilityText}";
        }
    }
}
