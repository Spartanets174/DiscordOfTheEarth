using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckSupportCardDisplay : OutlineClicableUI
{
    [Space, Header("Card Info")]
    [SerializeField]
    private Image supportImage;
    [SerializeField]
    private TextMeshProUGUI supportCardName;
    [SerializeField]
    private Image background;

    [Space]
    [SerializeField]
    private Button deleteButton;


    public event Action<CardSupport> onSupportCardDelete;

    private CardSupport currentCardSupport;
    public CardSupport CurrentCardSupport=> currentCardSupport;
    private void Awake()
    {
        IsEnabled = true;
        deleteButton.onClick.AddListener(DeleteCard);
        OnDoubleClick += x => DeleteCard();
    }
    private void OnDestroy()
    {
        deleteButton.onClick.RemoveListener(DeleteCard);
        OnDoubleClick -= x => DeleteCard();

    }
    public void SetData(CardSupport cardSupport, Sprite rarityBackground)
    {
        currentCardSupport = cardSupport;
        supportImage.sprite = cardSupport.image;
        supportCardName.text = cardSupport.cardName;
        background.sprite = rarityBackground;
    }

    private void DeleteCard()
    {
        onSupportCardDelete?.Invoke(currentCardSupport);
        Destroy(gameObject);
    }
}
