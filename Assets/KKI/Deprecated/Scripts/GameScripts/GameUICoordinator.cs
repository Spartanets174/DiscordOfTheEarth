using System;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUICoordinator : MonoBehaviour, ILoadable
{
    [Header("Scripts")]
    [SerializeField]
    private BattleSystem battleSystem;

    [Space, Header("UI Controllers")]
    [SerializeField]
    private ChosenCharacterDeatilsDisplay chosenCharacterDeatilsDisplay;
    [SerializeField]
    private PlayerControllerPresenter playerControllerPresenter;
    [SerializeField]
    private CardSupportAbilitiesController cardSupportAbilitiesController;
    private SettingsController settingsController;

    [Space, Header("Action UI")]
    [SerializeField]
    private Button endMoveButton;
    [SerializeField]
    private Button toMenuButton;
    [SerializeField]
    private Button setPlayerTurnButton;
    [SerializeField]
    private Button setEnemyTurnButton;
    [SerializeField]
    private Button setWinButton;
    [SerializeField]
    private Button setLostButton;
    [SerializeField]
    private Button toMenuButtonEndGame;

    [Space, Header("Info UI")]
    [SerializeField]
    private TextMeshProUGUI gameLog;
    [SerializeField]
    private TextMeshProUGUI pointsOfActionAndСube;
    [SerializeField]
    private TextMeshProUGUI endGameText;

    [Space, Header("GameObjects")]
    [SerializeField]
    private GameObject endGameInterface;
    [SerializeField]
    private GameObject DevButtons;
    [SerializeField]
    private GameObject gameInterface;

    [Space, Header("Colors")]
    [SerializeField]
    private Color enemyTextColor;
    [SerializeField]
    private Color playerTextColor;
    [SerializeField]
    private Color amountTextColor;
    [SerializeField]
    private Color changeTurnTextColor;

    private CompositeDisposable disposables = new();
    private CompositeDisposable disposablesForCards = new();
    private float timer = 1f;
    public void Init()
    {
        settingsController = FindObjectOfType<SettingsController>();

        cardSupportAbilitiesController.Init();
        playerControllerPresenter.Init();

        battleSystem.gameLogCurrentText.Subscribe(x =>
        {
            AddMessageToGameLog(x);
        }).AddTo(disposables);

        battleSystem.PointsOfAction.Subscribe(x =>
        {
            SetPointsOfActionAndСube(x);
        }).AddTo(disposables);

        battleSystem.CurrentChosenCharacter.Subscribe(x =>
        {
            SetChosenCharDeatils(x);
        });

        battleSystem.OnGameStarted += OnGameStarted;
        battleSystem.OnEnemyTurnStarted += OnEnemyTurnStart;
        battleSystem.OnPlayerTurnStarted += OnPlayerTurnStart;
        battleSystem.OnGameEnded += SetEndGame;
        settingsController.OnPauseStateChanged += SetPausedPlayer;


        DevButtons.SetActive(false);

        endMoveButton.onClick.AddListener(battleSystem.SetEnemyTurn);
        setPlayerTurnButton.onClick.AddListener(battleSystem.SetPlayerTurn);
        setEnemyTurnButton.onClick.AddListener(battleSystem.SetEnemyTurn);
        setWinButton.onClick.AddListener(battleSystem.SetWin);
        setLostButton.onClick.AddListener(battleSystem.SetLost);
        toMenuButton.onClick.AddListener(settingsController.TogglePausedState);
        toMenuButtonEndGame.onClick.AddListener(SceneController.ToMenu);



        cardSupportAbilitiesController.OnSupportAbilitySelected += OnSupportAbilitySelected;
        cardSupportAbilitiesController.OnSupportAbilityUsed += OnSupportAbilityUsed;
        cardSupportAbilitiesController.OnSupportAbilityUsingCancel += OnSupportAbilityUsed;

        chosenCharacterDeatilsDisplay.OnAbilitySelected += OnAbilitySelected;
        chosenCharacterDeatilsDisplay.OnAbilityUsed += OnAbilityUsed;
        chosenCharacterDeatilsDisplay.OnAbilityUsingCancel += OnAbilityUsed;

        Observable.EveryUpdate().Subscribe(x =>
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.V))
                {
                    DevButtons.SetActive(!DevButtons.activeSelf);
                    timer = 1f;
                    Debug.Log(DevButtons.activeSelf);
                }
            }
        }).AddTo(disposables);
    }

    private void OnAbilityUsed()
    {
        settingsController.CanPause = true;

        cardSupportAbilitiesController.EnableSupportCards();
    }

    private void OnAbilitySelected()
    {
        settingsController.CanPause = false;
        cardSupportAbilitiesController.DisableSupportCards();
    }

    private void OnSupportAbilityUsed()
    {
        settingsController.CanPause = true;

        chosenCharacterDeatilsDisplay.SetRulesAbilityButtonsState(chosenCharacterDeatilsDisplay.currentCharacter.Value);
    }

    private void OnSupportAbilitySelected()
    {
        settingsController.CanPause = false;

        chosenCharacterDeatilsDisplay.SetAbilityButtonsState(false);
        chosenCharacterDeatilsDisplay.currentCharacter.Value = null;
        battleSystem.CurrentChosenCharacter.Value = null;
    }

    private void OnGameStarted(Begin begin)
    {
        foreach (var gameCards in playerControllerPresenter.GameCharacterCardDisplays)
        {
            gameCards.OnClick += battleSystem.OnChooseCharacterButton;
            gameCards.OnCharacterSetted += CancelChoosing;
        }
        begin.OnCharacterMoved += playerControllerPresenter.GetChosenCard;
        begin.OnCharacterChosen += playerControllerPresenter.SetChosenCard;
        begin.OnCharacterChosen += AddCancelChoosing;
        begin.OnStateCompleted += OnBeginStateCompleted;
    }

    private void AddCancelChoosing(GameCharacterCardDisplay card)
    {
        settingsController.CanPause = false;
        Observable.EveryUpdate().Where(x => Input.GetKey(KeyCode.Escape) && card != null).Subscribe(x =>
        {
             CancelChoosing(card.gameObject);
        }).AddTo(disposablesForCards);
        card.OnClick -= battleSystem.OnChooseCharacterButton;
        card.OnClick += CancelChoosing;
    }
    private void CancelChoosing(GameObject card)
    {
        settingsController.CanPause = true;
        disposablesForCards.Dispose();
        disposablesForCards.Clear();
        disposablesForCards = new();
        GameCharacterCardDisplay cardDisplay = card.GetComponent<GameCharacterCardDisplay>();
        cardDisplay.OnClick -= CancelChoosing;
        if (battleSystem.PlayerController.PlayerCharactersObjects.Count!=5)
        {
            cardDisplay.OnClick += battleSystem.OnChooseCharacterButton;
        }
       
        battleSystem.FieldController.TurnOffCells();
        playerControllerPresenter.EbableUnspawnedCards();
    }
    private void OnBeginStateCompleted(State state)
    {
        Begin begin = (Begin)state;
        foreach (var gameCards in playerControllerPresenter.GameCharacterCardDisplays)
        {
            gameCards.OnClick -= battleSystem.OnChooseCharacterButton;
            gameCards.OnCharacterSetted -= CancelChoosing;
        }
        begin.OnCharacterMoved -= playerControllerPresenter.GetChosenCard;
        begin.OnCharacterChosen -= playerControllerPresenter.SetChosenCard;
        begin.OnCharacterChosen -= AddCancelChoosing;
        begin.OnStateCompleted -= OnBeginStateCompleted;

        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnDamaged += LogCharacterDamage;
            playerCharacter.OnDeath += LogDeath;
            playerCharacter.OnHeal += LogHeal;
        }

        foreach (var enemyCharacter in battleSystem.EnemyController.EnemyCharObjects)
        {
            enemyCharacter.OnDamaged += LogCharacterDamage;
            enemyCharacter.OnDeath += LogDeath;
            enemyCharacter.OnHeal += LogHeal;

        }
        foreach (var staticEnemyCharacter in battleSystem.EnemyController.StaticEnemyCharObjects)
        {
            staticEnemyCharacter.OnDamaged += LogCharacterDamage;
            staticEnemyCharacter.OnDeath += LogDeath;
            staticEnemyCharacter.OnHeal += LogHeal;

        }

        chosenCharacterDeatilsDisplay.Init();
    }


    private void SetEndGame()
    {
        if (battleSystem.State is Won)
        {
            endGameText.text = $"Поздравляем с победой, {battleSystem.PlayerController.PlayerDataController.CharacterName}! В награду вы получаете 3000 валюты!";
        }
        else
        {
            endGameText.text =  $"Увы, {battleSystem.PlayerController.PlayerDataController.CharacterName}, но вы проиграли! Но не отчаивайтесь, за старания мы дарим вам 500 валюты!";
        }
        
        endGameInterface.SetActive(true);
        gameInterface.SetActive(false);
    }

    private void OnDestroy()
    {
        endMoveButton.onClick.RemoveListener(battleSystem.SetEnemyTurn);
        setPlayerTurnButton.onClick.RemoveListener(battleSystem.SetPlayerTurn);
        setEnemyTurnButton.onClick.RemoveListener(battleSystem.SetEnemyTurn);
        setWinButton.onClick.RemoveListener(battleSystem.SetWin);
        setLostButton.onClick.RemoveListener(battleSystem.SetLost);
        toMenuButton.onClick.RemoveListener(SceneController.ToMenu);
        toMenuButtonEndGame.onClick.RemoveListener(SceneController.ToMenu);

        disposables.Dispose();
        disposables.Clear();
    }

    private void SetChosenCharDeatils(Character character)
    {
        chosenCharacterDeatilsDisplay.SetData(character);
    }
    private void OnPlayerTurnStart(PlayerTurn playerTurn)
    {
        AddMessageToGameLog($"<color=#{changeTurnTextColor.ToHexString()}>Ваш ход</color>");
        endMoveButton.interactable = true;
        foreach (var supportCard in cardSupportAbilitiesController.GameSupportCards)
        {
            supportCard.IsEnabled = true;
        }
        foreach (var Card in playerControllerPresenter.GameCharacterCardDisplays)
        {
            Card.IsEnabled = true;
        }
        foreach (var SupportCard in cardSupportAbilitiesController.GameSupportCards)
        {
            SupportCard.IsEnabled = true;
        }
    }

    private void SetPausedPlayer(bool state)
    {
        if (battleSystem.State is PlayerTurn playerTurn)
        {
            if (state)
            {
                playerTurn.ResetPlayer();
            }
            else
            {
                playerTurn.SetStateToNormal();
            }
        }
        /*if (battleSystem.State is EnemyTurn enemyTurn)
        {
            if (state)
            {
                battleSystem.EnemyController.StopTree();
            }
            else
            {
                battleSystem.EnemyController.RestartTree();
            }
        }*/
    }

    private void OnEnemyTurnStart(EnemyTurn enemyTurn)
    {
        endMoveButton.interactable = false;

        cardSupportAbilitiesController.DisableSupportCards();
        AddMessageToGameLog($"<color=#{changeTurnTextColor.ToHexString()}>Ход противника</color>");;

        foreach (var supportCard in cardSupportAbilitiesController.GameSupportCards)
        {
            supportCard.IsEnabled = false;
        }
        foreach (var Card in playerControllerPresenter.GameCharacterCardDisplays)
        {
            Card.IsEnabled = false;
        }

    }
    private void LogDeath(Character character)
    {
        if (character is PlayerCharacter)
        {
            AddMessageToGameLog($"Союзный юнит <color=#{playerTextColor.ToHexString()}>{character.CharacterName}</color>  убит");
        }
        else
        {
            AddMessageToGameLog($"Вражеский юнит <color=#{enemyTextColor.ToHexString()}>{character.CharacterName}</color> убит");
        }
    }

    private void LogHeal(Character healedCharacter, string characterUsedHeal, float healAmount)
    {
        Color characterColor = healedCharacter is PlayerCharacter? playerTextColor: enemyTextColor;

        SetChosenCharDeatils(healedCharacter);
        AddMessageToGameLog($"<color=#{characterColor.ToHexString()}>{characterUsedHeal}</color> восстанавливает юниту <color=#{characterColor.ToHexString()}>{healedCharacter.CharacterName}</color> <color=#{amountTextColor.ToHexString()}>{healAmount * 100:00.00}</color> единиц здоровья");
    }
    private void LogCharacterDamage(Character character, string enemyName, float finalDamage)
    {
        SetChosenCharDeatils(character);
        Color characterColor = character is PlayerCharacter ? playerTextColor : enemyTextColor;
        Color secondCharacterColor;
        if (battleSystem.State is PlayerTurn)
        {
            secondCharacterColor = character is PlayerCharacter ? enemyTextColor : playerTextColor;
        }
        else
        {
            secondCharacterColor = enemyTextColor;
        }

        if (finalDamage > 0)
        {
            AddMessageToGameLog($"<color=#{secondCharacterColor.ToHexString()}>{enemyName}</color> наносит <color=#{amountTextColor.ToHexString()}>{finalDamage * 100:00.00}</color> единиц урона юниту <color=#{characterColor.ToHexString()}>{character.CharacterName}</color>  ");
        }
        else
        {
            AddMessageToGameLog($"<color=#{characterColor.ToHexString()}>{character.CharacterName}</color> избежал получения урона от <color=#{secondCharacterColor.ToHexString()}>{enemyName}</color>");
        }
    }
    private void AddMessageToGameLog(string message)
    {
        gameLog.text = gameLog.text.Insert(0, message + "\n");
    }

    private void SetPointsOfActionAndСube(float value)
    {
        pointsOfActionAndСube.text = value.ToString();

    }
}
