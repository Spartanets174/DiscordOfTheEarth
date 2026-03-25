using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCharacterCardDisplay : OutlineClicableUI
{
    [Space, Header("Card Info")]
    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private Image classImage;
    [SerializeField]
    private Image background;
    [SerializeField]
    private TextMeshProUGUI characterName;

    [Space]
    [SerializeField]
    private Button deleteButton;

    public event Action<CharacterCard> onCharacterDelete;

    private CharacterCard currentCharacterCard;
    public CharacterCard CurrentCharacterCard => currentCharacterCard;
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
    public void SetData(CharacterCard characterCard, Sprite classImage, Sprite rarityBackground)
    {
        currentCharacterCard = characterCard;
        characterImage.sprite = characterCard.image;
        characterName.text = characterCard.cardName;
        background.sprite = rarityBackground;
        this.classImage.sprite = classImage;
    }

    private void DeleteCard()
    {
        onCharacterDelete?.Invoke(currentCharacterCard);
        Destroy(gameObject);
    }
}
