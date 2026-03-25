using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WallSupportCardAbility : BaseSupport—ardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    private SelectCellsBehaviour selectCellsBehaviour;
    private SpawnObjectBehaviour spawnObjectBehaviour;

    private GameObject wall;
    private WallSupportCardAbilityData abilityData;

    private List<GameObject> kostilGameObjects = new();

    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (WallSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectCellsBehaviour(abilityData.selectCardText, battleSystem, new Vector2(3, 1), "allowed"));
        SetUseCardBehaviour(new SpawnObjectBehaviour(abilityData.prefab));

        selectCellsBehaviour = (SelectCellsBehaviour)CardSelectBehaviour;
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
                spawnObjectBehaviour.rotation = selectCellsBehaviour.range.x > selectCellsBehaviour.range.y ? new Vector3(0, 180, 0) : new Vector3(0, 90, 0);

            }
            else
            {
                spawnObjectBehaviour.rotation = selectCellsBehaviour.range.x > selectCellsBehaviour.range.y ? Vector3.zero : new Vector3(0, 90, 0);
            }
            UseCard(selectCellsBehaviour.clickedCell.gameObject);
        }
        else
        {
            SelectCard();
        }
    }

    private void OnCardUse()
    {
        wall = spawnObjectBehaviour.spawnedPrefab;
        foreach (var cell in selectCellsBehaviour.highlightedCells)
        {
            GameObject gameObject = new GameObject("kostil");
            gameObject.transform.SetParent(cell.transform);
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
    public void ReturnToNormal()
    {
        foreach (var kostilGameObject in kostilGameObjects)
        {
            GameObject.Destroy(kostilGameObject);
        }
        kostilGameObjects.Clear();

        GameObject.Destroy(wall);
        OnReturnToNormal?.Invoke(this);
    }
}
[Serializable]
public class WallSupportCardAbilityData : BaseSupport—ardAbilityData
{
    public string selectCardText;

    public GameObject prefab;

    public int turnCount;

    [Header("ÕÂ ÚÓ„‡Ú¸!")]
    public bool isBuff;
}