using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICoordinator : MonoBehaviour, ILoadable
{
    [Space, Header("Buttons")]
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button bookOfCardsButton;
    [SerializeField]
    private Button exitbutton;

    [Space, Header("Texts")]
    [SerializeField]
    private TextMeshProUGUI playerNick;
    [SerializeField]
    private TextMeshProUGUI warningText;
    [SerializeField]
    private string shopCaption;
    [SerializeField]
    private string settingsCaption;

    [Space, Header("Game objects")]   
    [SerializeField]
    private GameObject bookOfCards;
    [SerializeField]
    private GameObject shop;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private Tooltip tooltip;

    [Space, Header("Interactable objects")]
    [SerializeField]
    private OutlineInteractableObject shopObject;
    [SerializeField]
    private OutlineInteractableObject settingsObject;

    [Space, Header("Controllers")]
    [SerializeField]
    private PlayerDataController PlayerManager;
    private SettingsController settingsController;

    public void Init()
    {
        settingsController = FindObjectOfType<SettingsController>();
        playButton.onClick.AddListener(ToGame);
        exitbutton.onClick.AddListener(SceneController.Exit);
        bookOfCardsButton.onClick.AddListener(TurnOnBookOfCards);

        shopObject.OnClick += TurnOnShop;
        shopObject.OnHoverEnter += TurnOnShopText;
        shopObject.OnHoverExit += x => tooltip.HideTooltip();
        shopObject.OnEnableChanged += x => { if (!x) tooltip.HideTooltip(); };


        settingsObject.OnClick += TurnOnSettings;
        settingsObject.OnHoverEnter += TurnOnSettingsText;
        settingsObject.OnHoverExit += x => tooltip.HideTooltip();
        settingsObject.OnEnableChanged += x => { if (!x) tooltip.HideTooltip(); };

        shopObject.IsEnabled = true;
        settingsObject.IsEnabled = true;

        playerNick.text = $"Приветсвуем тебя, {PlayerManager.CharacterName}!";
        tooltip.HideTooltip();
    }
    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(ToGame);
        exitbutton.onClick.RemoveListener(SceneController.Exit);
        bookOfCardsButton.onClick.RemoveListener(TurnOnBookOfCards);

        shopObject.OnClick -= TurnOnShop;
        shopObject.OnHoverEnter -= TurnOnShopText;
        shopObject.OnHoverExit -= x=> tooltip.HideTooltip();

        settingsObject.OnClick -= TurnOnSettings;
        settingsObject.OnHoverEnter -= TurnOnSettingsText;
        settingsObject.OnHoverExit -= x => tooltip.HideTooltip();

        StopAllCoroutines();
    }

    public void ToGame()
    {
        if (PlayerManager.DeckUserCharCards.Count < 5 && PlayerManager.DeckUserSupportCards.Count < 7)
        {
            SetState(warningText.gameObject, true);
            StartCoroutine(TurnOffWarnningText($"В колоде недостаточно карт персонажей ({5 - PlayerManager.DeckUserCharCards.Count}) и карт помощи ({7 - PlayerManager.DeckUserSupportCards.Count} )"));
            return;
        }
        if (PlayerManager.DeckUserCharCards.Count < 5)
        {
            SetState(warningText.gameObject, true);
            StartCoroutine(TurnOffWarnningText($"В колоде недостаточно карт персонажей ({5 - PlayerManager.DeckUserCharCards.Count})"));
            return;
        }
        if (PlayerManager.DeckUserSupportCards.Count < 7)
        {
            SetState(warningText.gameObject, true);
            StartCoroutine(TurnOffWarnningText($"В колоде недостаточно карт помощи ({7 - PlayerManager.DeckUserSupportCards.Count} )"));
            return;
        }

        SceneController.ToGame();
    }

    private void TurnOnBookOfCards()
    {
        shopObject.IsEnabled = false;
        settingsObject.IsEnabled = false;
        SetState(bookOfCards, true); 
    }
    private void TurnOnShop(GameObject gameObject)
    {
        shopObject.IsEnabled = false;
        settingsObject.IsEnabled = false;
        SetState(shop,true);
    }
    private void TurnOnSettings(GameObject gameObject)
    {
        settingsController.TogglePausedState();
    }
    private void TurnOnShopText(GameObject gameObject)
    {
        tooltip.ShowTooltip(shopCaption);
    }
    private void TurnOnSettingsText(GameObject gameObject)
    {
        tooltip.ShowTooltip(settingsCaption);
    }
    private IEnumerator TurnOffWarnningText(string text)
    {
        warningText.text = text;
        yield return new WaitForSecondsRealtime(2);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(warningText.DOFade(0, 2f))
        .OnComplete(() => {
            warningText.color = Color.red;
            warningText.gameObject.SetActive(false);
            mySequence.Kill();
        });
        mySequence.Play();
    }
    private void SetState(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }
}
