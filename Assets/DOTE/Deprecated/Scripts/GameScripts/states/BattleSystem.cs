using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;


public class BattleSystem : StateMachine, ILoadable
{

    [Header("Controllers")]
    [SerializeField]
    private PlayerController m_playerController;
    public PlayerController PlayerController => m_playerController;
    [SerializeField]
    private EnemyController enemyController;
    public EnemyController EnemyController => enemyController;
    [SerializeField]
    private FieldController m_fieldController;
    public FieldController FieldController => m_fieldController;

    [Header("Colors")]
    [SerializeField]
    public Color infoColor;
    [SerializeField]
    public Color enemyTextColor;
    [SerializeField]
    public Color playerTextColor;
    [SerializeField]
    public Color amountTextColor;

    private Dictionary<ITurnCountable, int> m_playerTurnCountables = new();
    public Dictionary<ITurnCountable, int> PlayerTurnCountables => m_playerTurnCountables;

    private Dictionary<ITurnCountable, int> m_enemyTurnCountables = new();
    public Dictionary<ITurnCountable, int> EnemyTurnCountables => m_enemyTurnCountables;

    public ReactiveProperty<Character> CurrentChosenCharacter = new ReactiveProperty<Character>();

    public ReactiveProperty<int> PointsOfAction = new();
    public int PointsOfActionValue => PointsOfAction.Value;

    public ReactiveProperty<string> gameLogCurrentText = new ReactiveProperty<string>();

    public Type StateType => State.GetType();

    public event Action<Begin> OnGameStarted;
    public event Action OnGameEnded;
    public event Action<PlayerTurn> OnPlayerTurnStarted;
    public event Action<EnemyTurn> OnEnemyTurnStarted;

    private CompositeDisposable disposables = new();
    private static BattleSystem m_instance;
    public static BattleSystem Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<BattleSystem>();
            }
            return m_instance;
        }
    }

    public void Init()
    {
        m_instance = this;
        FieldController.InvokeActionOnField(x => x.OnClick += x => FieldController.TurnOnCells());
        PlayerController.PlayerCharactersObjects.ObserveCountChanged().Subscribe(x =>
        {
            if (x == 5)
            {
                StartGame();
            }
            if (x == 0)
            {
                SetLost();
            }
        }).AddTo(disposables);

        EnemyController.EnemyCharObjects.ObserveCountChanged().Subscribe(x =>
        {
            if (x == 0)
            {
                SetWin();
            }
        }).AddTo(disposables);

        Begin begin = new(this);
        SetState(begin);
        OnGameStarted?.Invoke(begin);
    }

    private void OnDestroy()
    {
        disposables.Dispose();
        disposables.Clear();
    }

    public void OnChooseCharacterButton(GameObject character)
    {
        StartCoroutine(State.ChooseCharacter(character));
    }
    public void OnMoveButton(GameObject cell)
    {
        StartCoroutine(State.Move(cell));
    }

    public void OnAttackButton(GameObject target)
    {
        StartCoroutine(State.Attack(target));
    }
    public void OnSupportCardButton(GameObject cardSupport)
    {
        StartCoroutine(State.UseSupportCard(cardSupport));
    }
    public void OnAttackAbilityButton(GameObject gameObject)
    {
        StartCoroutine(State.UseAttackAbility(gameObject));
    }

    public void OnDefensiveAbilityButton(GameObject gameObject)
    {
        StartCoroutine(State.UseDefensiveAbility(gameObject));
    }

    public void OnBuffAbilityButton(GameObject gameObject)
    {
        StartCoroutine(State.UseBuffAbility(gameObject));
    }

    public void OnUseItemButton()
    {
        StartCoroutine(State.UseItem());
    }

    [ContextMenu("SetPlayerTurn")]
    public void SetPlayerTurn()
    {
        CurrentChosenCharacter.Value = null;
        /*EnemyController.StopTree();*/
        PlayerTurn playerTurn = new(this);
        SetState(playerTurn);
        OnPlayerTurnStarted?.Invoke(playerTurn);      
    }
    [ContextMenu("SetEnemyTurn")]
    public void SetEnemyTurn()
    {
        CurrentChosenCharacter.Value = null;
        EnemyTurn enemyTurn = new(this);
        SetState(enemyTurn);
        OnEnemyTurnStarted?.Invoke(enemyTurn);
        
    }
    public void SetWin()
    {
        /*EnemyController.StopTree();*/
        PlayerController.ClearDisposables();
        SetState(new Won(this));
        OnGameEnded?.Invoke();
    }
    public void SetLost()
    {
        /*EnemyController.StopTree();*/
        PlayerController.ClearDisposables();
        SetState(new Lost(this));
        OnGameEnded?.Invoke();
    }

    private void StartGame()
    {
        PlayerController.SetPlayerState(true, x =>
        {
            x.OnClick += SetCurrentChosenCharacter;
            x.OnPositionOnFieldChanged += EnemyController.AttackPlayerCharacterOnMove;
        });

        EnemyController.SetEnemiesState(true, (x) =>
        {
            x.OnClick += SetCurrentChosenCharacter;
            x.OnPositionOnFieldChanged += EnemyController.AttackEnemyCharacterOnMove;
        });
        EnemyController.SetStaticEnemiesState(true, (x) => { x.OnClick += SetCurrentChosenCharacter; });

        int cubeValue = UnityEngine.Random.Range(1, 6);

        gameLogCurrentText.Value = $"На кубике выпало <color=#{infoColor.ToHexString()}>{cubeValue}</color>";

        if (cubeValue % 2 == 0)
        {
            SetPlayerTurn();
        }
        else
        {
            SetEnemyTurn();
        }
    }

    public void SetCurrentChosenCharacter(GameObject character)
    {
        if (character != null)
        {
            Character tmpCharacter = character.GetComponent<Character>();
            if (CurrentChosenCharacter.Value != null)
            {
                CurrentChosenCharacter.Value.IsChosen = false;
                if (CurrentChosenCharacter.Value == tmpCharacter)
                {
                    CurrentChosenCharacter.Value = null;
                    return;
                }
            }

            CurrentChosenCharacter.Value = tmpCharacter;
            CurrentChosenCharacter.Value.IsChosen = true;
        }
        else
        {
            Debug.LogError("Нет персонажа");
        }
    }
}
