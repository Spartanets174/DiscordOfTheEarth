using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerControllerPresenter : MonoBehaviour, ILoadable
{
    [SerializeField]
    private PlayerController playerController;
    public PlayerController PlayerController=> playerController;

    [SerializeField]
    private BattleSystem battleSystem;

    [SerializeField]
    private GameCharacterCardDisplay gameCharacterCardPrefab;

    [SerializeField]
    private CardSupportAbilitiesController cardSupportAbilitiesController;

    [SerializeField]
    private Transform gameCharacterCardsParent;

    private List<GameCharacterCardDisplay> m_gameCharacterCards = new();
    public List<GameCharacterCardDisplay> GameCharacterCardDisplays => m_gameCharacterCards;

    private CompositeDisposable disposables = new();
    public void Init()
    {
        playerController.PlayerCharactersObjects.ObserveAdd().Subscribe(x => {
            SetChosenStateToCards(false);
            EbableUnspawnedCards();
        }).AddTo(disposables);

        foreach (var Card in playerController.PlayerDataController.DeckUserCharCards)
        {
            GameCharacterCardDisplay cardDisplay = Instantiate(gameCharacterCardPrefab, Vector3.zero, Quaternion.identity, gameCharacterCardsParent);
            cardDisplay.transform.localPosition = Vector3.zero;
            cardDisplay.SetData(Card);
            m_gameCharacterCards.Add(cardDisplay);
            cardDisplay.IsEnabled = true;
        }


        battleSystem.OnPlayerTurnStarted += OnPlayerTurnStart;
    }

    private void OnDestroy()
    {
        battleSystem.OnPlayerTurnStarted -= OnPlayerTurnStart;

        disposables.Dispose();
        disposables.Clear();
    }

    private void OnPlayerTurnStart(PlayerTurn playerTurn)
    {
        playerController.PlayerTurn = playerTurn;
        foreach (var gameSupportCArdDisplay in cardSupportAbilitiesController.GameSupportCards)
        {
            gameSupportCArdDisplay.GameSupportÑardAbility.OnUsingCancel += OnUsingCancel;
            gameSupportCArdDisplay.GameSupportÑardAbility.OnSupportCardAbilityUsed += OnSupportCardAbilityUsed;
            gameSupportCArdDisplay.GameSupportÑardAbility.OnSupportCardAbilityCharacterSelected += OnSupportCardAbilityCharacterSelected;
        }
        playerTurn.OnStateCompleted += OnPlayerTurnStartCompleted;
    }

    private void OnPlayerTurnStartCompleted(State state)
    {
        foreach (var gameSupportCArdDisplay in cardSupportAbilitiesController.GameSupportCards)
        {
            gameSupportCArdDisplay.GameSupportÑardAbility.OnUsingCancel -= OnUsingCancel;
            gameSupportCArdDisplay.GameSupportÑardAbility.OnSupportCardAbilityUsed -= OnSupportCardAbilityUsed;
            gameSupportCArdDisplay.GameSupportÑardAbility.OnSupportCardAbilityCharacterSelected -= OnSupportCardAbilityCharacterSelected;
        }

        playerController.PlayerTurn.OnStateCompleted -= OnPlayerTurnStartCompleted;
    }
    private void OnSupportCardAbilityCharacterSelected(ICharacterSelectable selectable)
    {
        playerController.PlayerTurn.ClearDisposables();
    }
    private void OnSupportCardAbilityUsed(ICardUsable usable)
    {
        playerController.PlayerTurn.SetStateToNormal();
        battleSystem.FieldController.TurnOnCells();
        playerController.PlayerTurn.ClearDisposables();

    }
    private void OnUsingCancel(BaseSupportÑardAbility ability)
    {
        playerController.PlayerTurn.SetStateToNormal();
        battleSystem.FieldController.TurnOnCells();
        playerController.PlayerTurn.ClearDisposables();

        foreach (var SupportCard in cardSupportAbilitiesController.GameSupportCards)
        {
            SupportCard.IsEnabled = true;
        }
        if (ability is ITurnCountable turnCountable)
        {
            if (turnCountable.IsBuff)
            {
                battleSystem.PlayerTurnCountables.Remove(turnCountable);
            }
            else
            {
                battleSystem.EnemyTurnCountables.Remove(turnCountable);
            }
        }
    }

    private void SetChosenStateToCards(bool State)
    {
        foreach (var item in m_gameCharacterCards)
        {
            item.IsChosen = State;
        }
    }

    public void EbableUnspawnedCards()
    {
        foreach (var item in m_gameCharacterCards)
        {
            if (!item.IsCharacterSpawned)
            {
                item.IsEnabled = true;
            }
        }
    }

    public void SetChosenCard(GameCharacterCardDisplay cardDisplay)
    {
        foreach (var item in m_gameCharacterCards)
        {
            item.IsChosen = false;
        }
        cardDisplay.IsChosen = true;
    }

    public GameCharacterCardDisplay GetChosenCard()
    {
        foreach (var item in m_gameCharacterCards)
        {
            if (item.IsChosen)
            {
                return item;
            }
        }
        return null;
    }
}
