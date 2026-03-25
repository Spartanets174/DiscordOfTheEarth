using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPresenter : CardPresenter, ILoadable
{
    [Space, Header("Prefabs")]
    [SerializeField]
    private CardDisplay characterCardObjectPrefab;
    [SerializeField]
    private CardSupportDisplay supportCardObjectPrefab;

    [Space, Header("Buttons")]
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private List<ButtonRace> raceButtons;
    [SerializeField]
    private List<ButtonClass> classButtons;
    [SerializeField]
    private List<ButtonSupportType> supportTypeButtons;

    [Space, Header("Sprites")]
    [SerializeField]
    private Sprite warrior;
    [SerializeField]
    private Sprite archer;
    [SerializeField]
    private Sprite cavalry;
    [SerializeField]
    private Sprite magician;
    [SerializeField]
    private Sprite normalBackground;
    [SerializeField]
    private Sprite mythBackground;


    private List<CardDisplay> characterCardObjects = new();
    private List<CardSupportDisplay> supportCardObjects = new();

    [SerializeField]
    private BuyCardDisplay buyCardDisplay;

    private ShopController shopController;
    public void Init()
    {
        shopController = FindObjectOfType<ShopController>();
        buyCardDisplay.BuyButton.onClick.AddListener(BuyCard);

        closeButton.onClick.AddListener(CloseShop);

        SpawnCharacterCards();
        SpawnSupportCards();
        foreach (var raceButton in raceButtons)
        {
            raceButton.OnButtonClick += RaceButtonCLick;
        }
        foreach (var classButton in classButtons)
        {
            classButton.OnButtonClick += ClassButtonCLick;
        }
        foreach (var supportTypeButton in supportTypeButtons)
        {
            supportTypeButton.OnButtonClick += SupportTypeButtonCLick;
        }
    }

    private void CloseShop()
    {
        gameObject.SetActive(false);
        foreach (var outlineInteractableObject in shopController.OutlineInteractableObjects)
        {
            outlineInteractableObject.IsEnabled = true;
        }
    }

    private void SetBuyCardWindow(GameObject CardObject)
    {
        buyCardDisplay.gameObject.SetActive(true);
        if (CardObject.TryGetComponent(out CardDisplay characterCard))
        {
            buyCardDisplay.SetCardInfo(characterCard.Card);
        }
        if (CardObject.TryGetComponent(out CardSupportDisplay cardSupport))
        {
            buyCardDisplay.SetCardInfo(cardSupport.CardSupport);
        }

    }
    private void BuyCard()
    {
        if (buyCardDisplay.ChosenCard is CharacterCard)
        {
            if (shopController.CanBuyCharacterCard(buyCardDisplay.ChosenCard as CharacterCard))
            {
                SpawnCharacterCards();
            }
            else
            {
                StartCoroutine(buyCardDisplay.TurnOffNotEnoughtCaption());
            }

        }
        if (buyCardDisplay.ChosenCard is CardSupport)
        {
            if (shopController.CanBuySupportCard(buyCardDisplay.ChosenCard as CardSupport))
            {
                SpawnSupportCards();
            }
            else
            {
                StartCoroutine(buyCardDisplay.TurnOffNotEnoughtCaption());
            }

        }
    }

    private void RaceButtonCLick(ButtonRace buttonRace)
    {
        foreach (var raceButton in raceButtons)
        {
            if (raceButton != buttonRace)
            {
                raceButton.IsEnabled = false;
            }

        }
        shopController.SetCurrentRace(buttonRace.Race);
        SpawnCharacterCards();
    }
    private void ClassButtonCLick(ButtonClass buttonClass)
    {
        foreach (var classButton in classButtons)
        {
            if (classButton != buttonClass)
            {
                classButton.IsEnabled = false;
            }

        }
        shopController.SetCurrentClass(buttonClass.Class);
        SpawnCharacterCards();
    }
    private void SupportTypeButtonCLick(ButtonSupportType buttonSupportType)
    {
        foreach (var supportTypeButton in supportTypeButtons)
        {
            if (buttonSupportType != supportTypeButton)
            {
                supportTypeButton.IsEnabled = false;
            }

        }
        shopController.SetCurrentTypeOfSupport(buttonSupportType.TypeOfSupport);
        SpawnSupportCards();
    }

    protected override void SpawnCharacterCards()
    {
        foreach (var characterCardObject in characterCardObjects)
        {
            Destroy(characterCardObject.gameObject);
        }
        characterCardObjects.Clear();

        List<CharacterCard> cards = shopController.FilterCharacterCards(shopController.CharacterShopCards);

        foreach (var card in cards)
        {
            CardDisplay cardObject = Instantiate(characterCardObjectPrefab, Vector3.zero, Quaternion.identity, parentToSpawnCharacterCards);
            cardObject.transform.localPosition = Vector3.zero;
            Sprite currentRarityBackground = null;
            Sprite currentClassImage = null;
            switch (card.rarity)
            {
                case Enums.Rarity.Обычная:
                    currentRarityBackground = normalBackground;
                    break;
                case Enums.Rarity.Мифическая:
                    currentRarityBackground = mythBackground;
                    break;
            }

            switch (card.Class)
            {
                case Enums.Classes.Паладин:
                    currentClassImage = warrior;
                    break;
                case Enums.Classes.Лучник:
                    currentClassImage = archer;
                    break;
                case Enums.Classes.Кавалерия:
                    currentClassImage = cavalry;
                    break;
                case Enums.Classes.Маг:
                    currentClassImage = magician;
                    break;
            }

            cardObject.SetValues(card, currentClassImage, currentRarityBackground);

            cardObject.OnClick += SetBuyCardWindow;
            characterCardObjects.Add(cardObject);
        }
    }



    protected override void SpawnSupportCards()
    {
        foreach (var supportCardObject in supportCardObjects)
        {
            Destroy(supportCardObject.gameObject);
        }
        supportCardObjects.Clear();

        List<CardSupport> cardsSupport = shopController.FilterSupportCards(shopController.SupportShopCards);


        foreach (var cardSupport in cardsSupport)
        {
            CardSupportDisplay cardSupportObject = Instantiate(supportCardObjectPrefab, Vector3.zero, Quaternion.identity, parentToSpawnSupportCards);
            cardSupportObject.transform.localPosition = Vector3.zero;
            Sprite currentRarityBackground = null;

            switch (cardSupport.rarity)
            {
                case Enums.Rarity.Обычная:
                    currentRarityBackground = normalBackground;
                    break;
                case Enums.Rarity.Мифическая:
                    currentRarityBackground = mythBackground;
                    break;
            }


            cardSupportObject.SetValues(cardSupport, currentRarityBackground);
            cardSupportObject.OnClick += SetBuyCardWindow;

            supportCardObjects.Add(cardSupportObject);
        }
    }
}
