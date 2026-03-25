using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


[Serializable]
public class PlayerTurn : State
{
    public event Action OnPlayerReseted;

    private List<Cell> cellsToMove = new();
    private List<Enemy> enemiesToAttack = new();

    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        BattleSystem.PointsOfAction.Value = 20;

        CheckPlayerTurnCountables();

        BattleSystem.PlayerController.ResetAllPlayerCharacters();
        BattleSystem.EnemyController.AttackAllPlayersStaticCharacters();

        OnStateStarted += OnPlayerTurnStarted;
        OnStateCompleted += OnPlayerTurnCompleted;

        new WaitForSecondsRealtime(1f);
     
        

     
        OnStateStartedInvoke();
        yield break;
    }


    //При выборе персонажа
    public override IEnumerator ChooseCharacter(GameObject character)
    {
        PlayerCharacter playerCharacter = character.GetComponent<PlayerCharacter>();

        foreach (var item in cellsToMove)
        {
            item.OnClick -= BattleSystem.OnMoveButton;
        }
        foreach (var item in enemiesToAttack)
        {
            item.OnClick -= BattleSystem.OnAttackButton;
        }

        BattleSystem.FieldController.TurnOffCells();
        cellsToMove = BattleSystem.FieldController.GetCellsForMove(playerCharacter, playerCharacter.Speed);
        SetEnemiesForAttack(playerCharacter);

        foreach (var item in cellsToMove)
        {
            item.OnClick += BattleSystem.OnMoveButton;
        }
        foreach (var item in enemiesToAttack)
        {
            item.OnClick += BattleSystem.OnAttackButton;
        }
        yield break;
    }

    public override IEnumerator Move(GameObject cell)
    {
        Cell cellToMove = cell.GetComponent<Cell>();
        PlayerCharacter playerCharacter = BattleSystem.PlayerController.CurrentPlayerCharacter;
        Cell currentCell = playerCharacter.GetComponentInParent<Cell>();

        int moveCost = BattleSystem.FieldController.GetMoveCost(currentCell,cellToMove, BattleSystem.State, playerCharacter);

        foreach (var item in cellsToMove)
        {
            item.OnClick -= BattleSystem.OnMoveButton;
        }
        foreach (var item in enemiesToAttack)
        {
            item.OnClick -= BattleSystem.OnAttackButton;
        }

        if (moveCost > BattleSystem.PointsOfAction.Value )
        {
            BattleSystem.gameLogCurrentText.Value = "Недостаточно очков действий";
            yield break;
        }

        if (moveCost > playerCharacter.Speed)
        {
            BattleSystem.gameLogCurrentText.Value = "Недостаточно скорости у персонажа";
            yield break;
        }   

        BattleSystem.PointsOfAction.Value -= moveCost;

        playerCharacter.Move(moveCost, cell.transform);

        if (BattleSystem.PointsOfAction.Value == 0)
        {
            BattleSystem.SetEnemyTurn();
        }
        yield break;
    }
    public override IEnumerator Attack(GameObject target)
    {
        if (BattleSystem.PointsOfAction.Value < 2)
        {
            BattleSystem.gameLogCurrentText.Value = "Недостаточно очков действий";
            yield break;
        }

        PlayerCharacter playerCharacter = BattleSystem.PlayerController.CurrentPlayerCharacter;
        Enemy currentTarget = target.GetComponent<Enemy>();

        playerCharacter.OnAttackInvoke();

        currentTarget.Damage(playerCharacter);


        foreach (var item in cellsToMove)
        {
            item.OnClick -= BattleSystem.OnMoveButton;
        }
        foreach (var item in enemiesToAttack)
        {
            item.OnClick -= BattleSystem.OnAttackButton;
        }

        if (BattleSystem.EnemyController.EnemyCharObjects.Count == 0)
        {
            BattleSystem.SetWin();
        }

        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance > playerCharacter.ChanceToFreeAttack)
        {
            BattleSystem.PointsOfAction.Value -= 2;
        }
        
        if (BattleSystem.PointsOfAction.Value == 0)
        {
            BattleSystem.SetEnemyTurn();
        }
        BattleSystem.FieldController.TurnOnCells();

        yield break;
    }
    public override IEnumerator UseSupportCard(GameObject gameObject)
    {

        ResetPlayer();

        GameSupportCardDisplay supportCardDisplay = gameObject.GetComponent<GameSupportCardDisplay>();

        Observable.EveryUpdate().Where(x => Input.GetKey(KeyCode.Escape)).Subscribe(x =>
        {
            supportCardDisplay.GameSupportСardAbility.CancelUsingCard();
        }).AddTo(disposables);

        if (supportCardDisplay.GameSupportСardAbility is ITurnCountable turnCountable)
        {
            if (turnCountable.IsBuff)
            {
                BattleSystem.PlayerTurnCountables.Add(turnCountable, turnCountable.TurnCount);
                BattleSystem.PlayerTurnCountables[turnCountable] --;
            }
            else
            {
                BattleSystem.EnemyTurnCountables.Add(turnCountable, turnCountable.TurnCount);
            }
        }

        
        supportCardDisplay.GameSupportСardAbility.SelectCard();
        yield break;
    }


    public override IEnumerator UseAttackAbility(GameObject gameObject)
    {
        if (BattleSystem.PointsOfAction.Value < 11)
        {
            BattleSystem.gameLogCurrentText.Value = "Недостаточно очков действий для применения способности";
            yield break;
        }

        PlayerCharacter playerCharacter = gameObject.GetComponent<PlayerCharacter>();

        UseAbility(playerCharacter.AttackCharacterAbility);

        playerCharacter.UseAtackAbility();
        yield break;
    }
    public override IEnumerator UseDefensiveAbility(GameObject gameObject)
    {
        if (BattleSystem.PointsOfAction.Value < 11)
        {
            BattleSystem.gameLogCurrentText.Value = "Недостаточно очков действий для применения способности";
            yield break;
        }

        PlayerCharacter playerCharacter = gameObject.GetComponent<PlayerCharacter>();

        UseAbility(playerCharacter.DefenceCharacterAbility);

        playerCharacter.UseDefenceAbility();
        yield break;
    }
    public override IEnumerator UseBuffAbility(GameObject gameObject)
    {
        if (BattleSystem.PointsOfAction.Value < 11)
        {
            BattleSystem.gameLogCurrentText.Value = "Недостаточно очков действий для применения способности";
            yield break;
        }

        PlayerCharacter playerCharacter = gameObject.GetComponent<PlayerCharacter>();

        UseAbility(playerCharacter.BuffCharacterAbility);

        playerCharacter.UseBuffAbility();
        yield break;
    }

    private void UseAbility(BaseCharacterAbility baseCharacterAbility)
    {
        ResetPlayer();

        Observable.EveryUpdate().Where(x => Input.GetKey(KeyCode.Escape)).Subscribe(x =>
        {
            baseCharacterAbility.CancelUsingCard();
        }).AddTo(disposables);

        if (baseCharacterAbility is ITurnCountable turnCountable)
        {
            if (turnCountable.IsBuff)
            {
                BattleSystem.PlayerTurnCountables.Add(turnCountable, turnCountable.TurnCount);
                BattleSystem.PlayerTurnCountables[turnCountable]--;
            }
            else
            {
                BattleSystem.EnemyTurnCountables.Add(turnCountable, turnCountable.TurnCount);
            }
        }
    }

    public override IEnumerator UseItem()
    {
        /*Логика при применении предмета*/
        yield break;
    }
    public void OnPlayerTurnStarted()
    {
        SetStateToNormal();

        BattleSystem.FieldController.TurnOnCells();
    }

    public void OnPlayerTurnCompleted(State state)
    {
        ResetPlayer();
        BattleSystem.FieldController.TurnOnCells();
    }


    public void ResetPlayer()
    {
        BattleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= BattleSystem.OnChooseCharacterButton;
        });

        foreach (var cellToMove in cellsToMove)
        {
            cellToMove.OnClick -= BattleSystem.OnMoveButton;
        }
        foreach (var enemyToAttack in enemiesToAttack)
        {
            enemyToAttack.OnClick -= BattleSystem.OnAttackButton;
        }
        BattleSystem.FieldController.TurnOffCells();
        cellsToMove.Clear();
        enemiesToAttack.Clear();
        OnPlayerReseted?.Invoke();
    }

    public void ClearDisposables()
    {
        disposables.Dispose();
        disposables.Clear();
        disposables = new();
    }
    public void SetStateToNormal()
    {
        foreach (var playerCharacter in BattleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick += BattleSystem.OnChooseCharacterButton;
        }

        BattleSystem.PlayerController.SetPlayerStates(true, false);
        BattleSystem.EnemyController.SetEnemiesStates(true, false);
        BattleSystem.EnemyController.SetStaticEnemiesState(true);
    }

    private void CheckPlayerTurnCountables()
    {       
        List<ITurnCountable> playerTurnCountables = new List<ITurnCountable>();
        foreach (var playerTurnCountable in BattleSystem.PlayerTurnCountables)
        {
            playerTurnCountables.Add(playerTurnCountable.Key);
        }
        foreach (var key in playerTurnCountables)
        {
            if (BattleSystem.PlayerTurnCountables[key] == 0)
            {
                key.ReturnToNormal();
                BattleSystem.PlayerTurnCountables.Remove(key);
                CheckPlayerTurnCountables();
                break;
            }
            else
            {
                BattleSystem.PlayerTurnCountables[key]--;
            }
        }
    }

    private void SetEnemiesForAttack(Character character)
    {
        enemiesToAttack.Clear();
        if (character.IsAttackedOnTheMove) return;
        if (!character.CanDamage) return;

        SetAttackableCells(character.PositionOnField, Enums.Directions.top, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.bottom, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.right, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.left, character);
    }
    private void SetAttackableCells(Vector2 pos, Enums.Directions direction, Character character)
    {
        int newI = (int)pos.x;
        int newJ = (int)pos.y;

        for (int i = 0; i < character.Range; i++)
        {
            switch (direction)
            {
                case Enums.Directions.top:
                    newI--;
                    break;
                case Enums.Directions.bottom:
                    newJ--;
                    break;
                case Enums.Directions.right:
                    newI++;
                    break;
                case Enums.Directions.left:
                    newJ++;
                    break;
            }

            if (newI >= 7 || newI < 0)
            {
                break;
            }
            if (newJ >= 11 || newJ < 0)
            {
                break;
            }

            Cell cell = BattleSystem.FieldController.GetCell(newI, newJ);
            Enemy enemy = cell.GetComponentInChildren<Enemy>();
            KostilEnemy kostilEnemy = cell.GetComponentInChildren<KostilEnemy>();
            if (cell.transform.childCount > 1)
            {
                if (enemy != null&&enemy is not KostilEnemy)
                {
                    cell.SetColor("attack");
                    enemiesToAttack.Add(enemy);
                }
                if (kostilEnemy!=null)
                {
                    cell.SetColor("attack");
                    enemiesToAttack.Add(kostilEnemy.WallEnemyCharacter);
                }
                if (character.Class == Enums.Classes.Маг)
                {
                    continue;
                }
                else
                {
                    break;
                }

            }

        }
    }

}