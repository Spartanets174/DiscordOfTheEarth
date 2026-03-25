using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PeasantWithPitchforkCharacterDefenceAbility : BaseCharacterAbility
{
    private PeasantWithPitchforkCharacterDefenceAbilityData abilityData;
    private SelectCellsInRangeBehaviour selectCellsBehaviour;
    private SpawnObjectBehaviour spawnObjectBehaviour;

    private WallEnemyCharacter wallEnemyCharacter;

    private List<GameObject> kostilGameObjects = new();
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (PeasantWithPitchforkCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsInRangeBehaviour(abilityData.selectAbilityText, battleSystem, abilityOwner, new Vector2(3, 1), abilityData.range, "allowed"));
        SetUseCardBehaviour(new SpawnObjectBehaviour(abilityData.prefab));

        selectCellsBehaviour = (SelectCellsInRangeBehaviour)CardSelectBehaviour;
        spawnObjectBehaviour = (SpawnObjectBehaviour)UseCardBehaviour;

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        foreach (var item in selectCellsBehaviour.highlightedCells)
        {
            item.OnClick -= selectCellsBehaviour.OnSelectedInvoke;
        }
        battleSystem.FieldController.InvokeActionOnField(selectCellsBehaviour.UnSubscribe);

        if (selectCellsBehaviour.highlightedCells.Where(x => x.transform.childCount > 1).ToList().Count == 0 && selectCellsBehaviour.highlightedCells.Count == 3)
        {
            if (battleSystem.State is PlayerTurn)
            {
                spawnObjectBehaviour.rotation = selectCellsBehaviour.size.x > selectCellsBehaviour.size.y ? new Vector3(0, 180, 0) : new Vector3(0, 90, 0);

            }
            else
            {
                spawnObjectBehaviour.rotation = selectCellsBehaviour.size.x > selectCellsBehaviour.size.y ? Vector3.zero : new Vector3(0, 90, 0);

            }
            UseCard(selectCellsBehaviour.clickedCell.gameObject);
        }
        else
        {
            SelectCard();
        }
    }

    private void OnWallDeath(Character character)
    {
        GameObject.Destroy(wallEnemyCharacter.gameObject);
        foreach (var kostilGameObject in kostilGameObjects)
        {
            GameObject.Destroy(kostilGameObject);
        }
        kostilGameObjects.Clear();
    }

    private void OnCardUse()
    {
        wallEnemyCharacter = spawnObjectBehaviour.spawnedPrefab.GetComponent<WallEnemyCharacter>();
        wallEnemyCharacter.OnDeath += OnWallDeath;
        wallEnemyCharacter.OnClick += battleSystem.SetCurrentChosenCharacter;
        foreach (var cell in selectCellsBehaviour.highlightedCells)
        {
            GameObject gameObject = new GameObject("kostil");
            gameObject.transform.SetParent(cell.transform);
            gameObject.AddComponent<KostilEnemy>();
            gameObject.GetComponent<KostilEnemy>().WallEnemyCharacter = wallEnemyCharacter;
            kostilGameObjects.Add(gameObject);
        }

        battleSystem.FieldController.TurnOnCells();
    }

    private void OnCancelSelection()
    {
        foreach (var item in selectCellsBehaviour.highlightedCells)
        {
            item.OnClick -= selectCellsBehaviour.OnSelectedInvoke;
        }
        battleSystem.FieldController.TurnOnCells();
    }
}
[Serializable]
public class PeasantWithPitchforkCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public int range;

    public GameObject prefab;
}