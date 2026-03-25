using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class CardSupportAbilitiesController : MonoBehaviour, ILoadable
{
    [Header("Scripts")]
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private BattleSystem battleSystem;

    [Space, Header("Parents")]
    [SerializeField]
    private Transform gameSupportCardsParent;

    [Space, Header("Prefabs")]
    [SerializeField]
    private GameSupportCardDisplay gameSupportCardPrefab;

    [Space, Header("Texts")]
    [SerializeField]
    private TextMeshProUGUI tipsText;

    [Space, Header("Game objects")]
    [SerializeField]
    private GameObject topBlocker;
    [SerializeField]
    private GameObject bottomBlocker;
    [SerializeField]
    private GameObject tipsTextParent;
    [SerializeField]
    private GameObject centerBlocker;

    private CompositeDisposable disposables = new();

    private List<GameSupportCardDisplay> m_gameSupportCards = new();
    public List<GameSupportCardDisplay> GameSupportCards => m_gameSupportCards;

    private ReactiveProperty<GameSupportCardDisplay> currentGameSupportCardDisplay = new();
    public event Action OnSupportAbilitySelected;
    public event Action OnSupportAbilityUsed;
    public event Action OnSupportAbilityUsingCancel;
    public void Init()
    {
        foreach (var SupportCard in playerController.PlayerDataController.DeckUserSupportCards)
        {
            GameSupportCardDisplay cardDisplay = Instantiate(gameSupportCardPrefab, Vector3.zero, Quaternion.identity, gameSupportCardsParent);
            cardDisplay.transform.localPosition = Vector3.zero;


            cardDisplay.SetData(SupportCard, battleSystem);
            cardDisplay.DragAndDropComponent.StartPos = cardDisplay.transform.localPosition;

            StartCoroutine(SetPosDelayed(cardDisplay.DragAndDropComponent));


            cardDisplay.IsEnabled = false;

            cardDisplay.DragAndDropComponent.OnBeginDragEvent += OnBeginDrag;
            cardDisplay.DragAndDropComponent.OnEndDragEvent += OnEndDrag;
            cardDisplay.DragAndDropComponent.OnDropEvent += OnDropEvent;


            if (cardDisplay.GameSupport—ardAbility != null)
            {
                cardDisplay.GameSupport—ardAbility.OnUsingCancel += OnUsingCancel;
                cardDisplay.GameSupport—ardAbility.OnSecondSupportCardAbilitySelected += OnSecondSupportCardAbilitySelected;
                cardDisplay.GameSupport—ardAbility.OnSupportCardAbilityCharacterSelected += OnSupportCardAbilityCharacterSelected;
                cardDisplay.GameSupport—ardAbility.OnSupportCardAbilityUsed += OnSupportCardAbilityUsed;
            }
            m_gameSupportCards.Add(cardDisplay);
        }

        foreach (var item in GameSupportCards)
        {
            item.DragAndDropComponent.OnDropEvent += battleSystem.OnSupportCardButton;
            item.DragAndDropComponent.OnDropEvent += (x) => battleSystem.FieldController.TurnOnCells(); ;
        }

        currentGameSupportCardDisplay.Where(x => x != null).Subscribe(x =>
        {
            SetTipsText(x.GameSupport—ardAbility.CardSelectBehaviour.SelectCardTipText);
            OnSupportAbilitySelected?.Invoke();
        }).AddTo(disposables);
        battleSystem.OnPlayerTurnStarted += OnPlayerTurnStarted;

        SetBlockersState(false);
        tipsTextParent.SetActive(false);
    }

    private void OnPlayerTurnStarted(PlayerTurn turn)
    {
        turn.IsSupportCardUsed = false;
        EnableSupportCards();      
    }


    private void OnDestroy()
    {
        foreach (var cardDisplay in m_gameSupportCards)
        {
            cardDisplay.DragAndDropComponent.OnBeginDragEvent -= OnBeginDrag;
            cardDisplay.DragAndDropComponent.OnEndDragEvent -= OnEndDrag;
            cardDisplay.DragAndDropComponent.OnDropEvent -= OnDropEvent;


            if (cardDisplay.GameSupport—ardAbility != null)
            {
                cardDisplay.GameSupport—ardAbility.OnUsingCancel -= OnUsingCancel;
                cardDisplay.GameSupport—ardAbility.OnSecondSupportCardAbilitySelected -= OnSecondSupportCardAbilitySelected;
                cardDisplay.GameSupport—ardAbility.OnSupportCardAbilityCharacterSelected -= OnSupportCardAbilityCharacterSelected;
                cardDisplay.GameSupport—ardAbility.OnSupportCardAbilityUsed -= OnSupportCardAbilityUsed;
            }
        }
        battleSystem.OnPlayerTurnStarted -= OnPlayerTurnStarted;

        disposables.Dispose();
        disposables.Clear();
    }

    private void OnSecondSupportCardAbilitySelected(ICardSelectable selectable)
    {
        SetTipsText($"{selectable.SelectCardTipText}");
    }

    private void OnUsingCancel(BaseSupport—ardAbility ability)
    {
        tipsTextParent.SetActive(false);
        SetTipsText("");
        currentGameSupportCardDisplay.Value.gameObject.SetActive(true);
        currentGameSupportCardDisplay.Value = null;
        OnSupportAbilityUsingCancel?.Invoke();
    }

    private void OnSupportCardAbilityCharacterSelected(ICharacterSelectable uharacterSelectable)
    {
        SetTipsText($"{uharacterSelectable.SelectCharacterTipText}");
    }

    private void OnSupportCardAbilityUsed(ICardUsable usable)
    {
        GameSupportCards.Remove(currentGameSupportCardDisplay.Value);
        tipsTextParent.SetActive(false);
        SetBlockersState(false);
        SetTipsText("");

        battleSystem.State.IsSupportCardUsed = true;

        foreach (var cardDisplay in GameSupportCards)
        {
            cardDisplay.DragAndDropComponent.StartPos = cardDisplay.DragAndDropComponent.transform.localPosition;
        }
        OnSupportAbilityUsed?.Invoke();
    }
    private void OnDropEvent(GameObject gameObject)
    {
        DisableSupportCards();
        gameObject.SetActive(false);
        SetBlockersState(false);
        currentGameSupportCardDisplay.Value = gameObject.GetComponent<GameSupportCardDisplay>();
    }
    private void OnBeginDrag(GameObject gameObject)
    {
        tipsTextParent.SetActive(true);
        SetBlockersState(true);
        SetTipsText("œÂÂÚ‡˘ËÚÂ Í‡ÚÛ ‚ Ó·Î‡ÒÚ¸");
    }

    private void OnEndDrag(GameObject gameObject)
    {
        tipsTextParent.SetActive(false);
        SetBlockersState(false);
        SetTipsText("");
    }

    private void SetTipsText(string message)
    {
        tipsText.text = message;
    }
    private void SetBlockersState(bool state)
    {
        topBlocker.SetActive(state);
        bottomBlocker.SetActive(state);
        centerBlocker.SetActive(state);
    }
    public void EnableSupportCards()
    {
        if (!battleSystem.State.IsSupportCardUsed)
        {
            SetEnabledToSupportCards(true);
        }
    }
    public void DisableSupportCards()
    {
        SetEnabledToSupportCards(false);
    }
    private void SetEnabledToSupportCards(bool state)
    {
        foreach (var SupportCard in GameSupportCards)
        {
            SupportCard.IsEnabled = state;
            SupportCard.DragAndDropComponent.IsAllowedToDrag = state;
        }

    }
    private IEnumerator SetPosDelayed(DragAndDropComponent dragAndDropComponent)
    {
        yield return new WaitForEndOfFrame();
        dragAndDropComponent.StartPos = dragAndDropComponent.transform.localPosition;
    }

}
