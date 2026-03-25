using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/*using Tree = BehaviourTree.Tree;*/

public class EnemyController : MonoBehaviour, ILoadable
{
    [Header("Main sripts")]
    [SerializeField]
    private FieldController fieldController;
    public FieldController FieldController => fieldController;

    [SerializeField]
    private PlayerData playerData;

    [Header("Static enemy cards")]
    [SerializeField]
    private CharacterCard assasinCard;
    [SerializeField]
    private CharacterCard goliafCard;
    [SerializeField]
    private CharacterCard elementalCard;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject defaultEnemyPrefab;


    private ReactiveCollection<EnemyCharacter> m_enemyCharObjects = new();
    public ReactiveCollection<EnemyCharacter> EnemyCharObjects => m_enemyCharObjects;

    private ReactiveCollection<StaticEnemyCharacter> m_staticEnemyCharObjects = new();
    public ReactiveCollection<StaticEnemyCharacter> StaticEnemyCharObjects => m_staticEnemyCharObjects;

    private List<CharacterCard> m_enemyCharCards = new();
    public List<CharacterCard> EnemyCharCards => m_enemyCharCards;

    private EnemyCharacter m_currentEnemyCharacter;
    public EnemyCharacter CurrentEnemyCharacter => m_currentEnemyCharacter;

    public void Init()
    {
        CreateEnemy();
        InstantiateEnemies();
    }
    /*public override void SetupTree()
    {
        _root = new Sequence(new List<Node>
                {
                    new ChooseCharacter(this, battleSystem),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>{
                            new CheckCellsForAttack(battleSystem,this),
                            new Attack(battleSystem,this),
                        }),
                        new Sequence(new List<Node>{
                            new CheckCellsForMove(battleSystem,this),
                            new Movement(battleSystem,this),
                        }),
                    })

                });
    }

    public override void RestartTree()
    {
        if (_root != null)
        {
            _root.Evaluate();
        }
        else
        {
            SetupTree();
        }
    }*/
    private void CreateEnemy()
    {
        while (m_enemyCharCards.Count < 5)
        {
            CharacterCard EnemyMan = playerData.allCharCards[UnityEngine.Random.Range(0, playerData.allCharCards.Count)];
            if (!m_enemyCharCards.Contains(EnemyMan) && EnemyMan.characterPrefab != null)
            {
                m_enemyCharCards.Add(EnemyMan);
            }

        }
    }
    private void InstantiateEnemies()
    {
        int count = 0;
        //Спавн статических врагов
        for (int i = 0; i < fieldController.CellsOfFieled.GetLength(0); i++)
        {
            for (int j = 0; j < fieldController.CellsOfFieled.GetLength(1); j++)
            {
                //Спавн ассасинов
                if ((j == 4 || j == 6) && (i == 0 || i == 6))
                {
                    InstantiateStaticEnemy(assasinCard, fieldController.GetCell(i, j), j == 6);
                }
                //Спавн голиафов
                if ((j == 4 || j == 6) && (i == 2 || i == 4))
                {
                    InstantiateStaticEnemy(goliafCard, fieldController.GetCell(i, j), j == 6);
                }
                //Спавн элементалей
                if ((j == 2 || j == 8) && i % 2 != 0)
                {
                    InstantiateStaticEnemy(elementalCard, fieldController.GetCell(i, j), j == 8);
                }
            }
        }
        //Спавн двигающихся врагов
        while (count < 5)
        {
            Cell Cell = fieldController.GetCell(UnityEngine.Random.Range(0, fieldController.CellsOfFieled.GetLength(0)), UnityEngine.Random.Range(0, 2));
            if (!IsEnemyOnCell(Cell))
            {
                GameObject prefab;
                prefab = Instantiate(m_enemyCharCards[count].characterPrefab, Vector3.zero, Quaternion.identity, Cell.transform);
                EnemyCharacter enemyCharacter = prefab.AddComponent<EnemyCharacter>();
                enemyCharacter.transform.localPosition = Vector3.zero;
                enemyCharacter.OnDeath += OnEnemyCharacterDeath;

                enemyCharacter.SetData(m_enemyCharCards[count], count);

                m_enemyCharObjects.Add(enemyCharacter);

                count++;
            }
        }

    }
    private bool IsEnemyOnCell(Cell cell)
    {
        if (cell.transform.childCount > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void InstantiateStaticEnemy(CharacterCard characterCard, Cell cell, bool enemySide)
    {
        GameObject prefab = Instantiate(characterCard.characterPrefab, Vector3.zero, Quaternion.identity, cell.transform);
        StaticEnemyCharacter staticEnemyCharacter = prefab.AddComponent<StaticEnemyCharacter>();
        staticEnemyCharacter.transform.localPosition = Vector3.zero;

        if (!enemySide)
        {
            staticEnemyCharacter.transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        staticEnemyCharacter.SetData(characterCard, m_staticEnemyCharObjects.Count - 1);
        staticEnemyCharacter.OnDeath += OnStaticEnemyCharacterDeath;

        SetAttackableCells(Enums.Directions.top, staticEnemyCharacter);
        SetAttackableCells(Enums.Directions.bottom, staticEnemyCharacter);
        SetAttackableCells(Enums.Directions.right, staticEnemyCharacter);
        SetAttackableCells(Enums.Directions.left, staticEnemyCharacter);

        m_staticEnemyCharObjects.Add(staticEnemyCharacter);
    }
    private void OnEnemyCharacterDeath(Character character)
    {
        EnemyCharObjects.Remove((EnemyCharacter)character);
        Destroy(character.gameObject);
    }
    private void OnStaticEnemyCharacterDeath(Character character)
    {
        StaticEnemyCharObjects.Remove((StaticEnemyCharacter)character);
        Destroy(character.gameObject);
    }

    private void SetAttackableCells(Enums.Directions direction, StaticEnemyCharacter staticEnemyCharacter)
    {
        int newI = (int)staticEnemyCharacter.PositionOnField.x;
        int newJ = (int)staticEnemyCharacter.PositionOnField.y;

        for (int i = 0; i < staticEnemyCharacter.Range; i++)
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

            Cell cell = fieldController.GetCell(newI, newJ);

            if (cell.transform.childCount == 1)
            {
                staticEnemyCharacter.CellsToAttack.Add(cell);
            }

        }
    }


    public void SetCurrentEnemyChosenCharacter(EnemyCharacter character)
    {
        if (character != null)
        {
            foreach (var enemyCharacter in EnemyCharObjects)
            {
                enemyCharacter.IsCurrentEnemyCharacter = false;
            }
            m_currentEnemyCharacter = character;
            m_currentEnemyCharacter.IsCurrentEnemyCharacter = true;
        }
        else
        {
            Debug.LogError("Нет персонажа");
        }
    }

    public void SetEnemiesStates(bool state, bool chosenState, Action<EnemyCharacter> subAction = null)
    {
        foreach (var enemyCharacter in m_enemyCharObjects)
        {
            enemyCharacter.IsEnabled = state;
            enemyCharacter.IsChosen = chosenState;
            subAction?.Invoke(enemyCharacter);
        }
    }

    public void SetEnemiesState(bool state, Action<EnemyCharacter> subAction = null)
    {
        foreach (var enemyCharacter in m_enemyCharObjects)
        {
            enemyCharacter.IsEnabled = state;
            subAction?.Invoke(enemyCharacter);
        }
    }

    public void SetEnemiesChosenState(bool state, Action<EnemyCharacter> subAction = null)
    {
        foreach (var enemyCharacter in m_enemyCharObjects)
        {
            enemyCharacter.IsChosen = state;
            subAction?.Invoke(enemyCharacter);
        }
    }

    public void SetStaticEnemiesState(bool state, Action<StaticEnemyCharacter> subAction = null)
    {
        foreach (var staticEnemyCharacter in m_staticEnemyCharObjects)
        {
            staticEnemyCharacter.IsEnabled = state;
            subAction?.Invoke(staticEnemyCharacter);
        }
    }

    public void ResetEnemyCharacters()
    {
        foreach (var enemyCharacter in m_enemyCharObjects)
        {
            enemyCharacter.ResetCharacter();
        }
    }

    public void RemoveDebuffsAllEnemyCharacters()
    {
        foreach (var enemyCharacter in m_enemyCharObjects)
        {
            enemyCharacter.RemoveDebuffs();
        }
    }

    public void AttackEnemyCharacterOnMove(Character character)
    {
        foreach (var staticEnemy in m_staticEnemyCharObjects)
        {
            staticEnemy.AttackEnemyCharacter((EnemyCharacter)character);
        }
    }

    public void AttackPlayerCharacterOnMove(Character character)
    {

        foreach (var staticEnemy in m_staticEnemyCharObjects)
        {
            staticEnemy.AttackPlayerCharacter((PlayerCharacter)character);
        }
    }

    public void AttackAllEnemiesStaticCharacters()
    {
        foreach (var staticEnemy in m_staticEnemyCharObjects)
        {
            staticEnemy.AttackEnemyCharacters();
        }
    }

    public void AttackAllPlayersStaticCharacters()
    {
        foreach (var staticEnemy in m_staticEnemyCharObjects)
        {
            staticEnemy.AttackPlayerCharacters();
        }
    }
}
