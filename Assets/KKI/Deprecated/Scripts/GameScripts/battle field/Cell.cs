
using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

public class Cell : ChildOutlineInteractableObject
{
    [SerializeField]
    private ColorDictioanary colorDictionary;

    [SerializeField]
    private int transitionCost;

    [SerializeField]
    private bool m_isSwamp;
    public bool IsSwamp => m_isSwamp;

    private int m_transitionCostEnemy;
    public int TransitionCostEnemy
    {
        get => m_transitionCostEnemy;
        set => m_transitionCostEnemy = value;
    }
    private int m_transitionCostPlayer;
    public int TransitionCostPlayer
    {
        get => m_transitionCostPlayer;
        set => m_transitionCostPlayer = value;
    }

    private MeshRenderer meshRenderer;
    public MeshRenderer MeshRenderer => meshRenderer;

    private Vector2 m_cellIndex;
    public Vector2 CellIndex
    {
        get => m_cellIndex;
        set => m_cellIndex=value;
    }

    protected override void Awake()
    {
        base.Awake();
        TransitionCostEnemy = transitionCost;
        TransitionCostPlayer = transitionCost;

        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void SetCellState(bool state)
    {
        IsEnabled = state;
    }

    public void SetColor(string key)
    {
        meshRenderer.material = colorDictionary[key].GetColor((CellIndex.x + CellIndex.y) % 2 == 0);
    }
    public void SetCellMovable()
    {
        SetCellState(true);
        SetColor("allowed");
    }
}

[Serializable]
public class ColorDictioanary : SerializableDictionaryBase<string, SwapColor> { }

[Serializable]
public class SwapColor
{
    [SerializeField]
    private Material baseColor ;
    [SerializeField]
    private Material offsetColor;

    public Material GetColor(bool isOffset)
    {
        return isOffset ? offsetColor : baseColor;
    }
}