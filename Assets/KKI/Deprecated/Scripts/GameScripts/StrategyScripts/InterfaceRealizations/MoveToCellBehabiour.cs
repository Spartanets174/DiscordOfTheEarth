using System;
using UnityEngine;

public class MoveToCellBehaviour : ICardUsable
{
    private BattleSystem battleSystem;
    private float moveTime;

    public event Action OnCardUse;

    public MoveToCellBehaviour(BattleSystem battleSystem, float moveTime = 0.5f)
    {
        this.battleSystem = battleSystem;
        this.moveTime = moveTime;
    }

    public void UseAbility(GameObject gameObject)
    {
        Cell currentCell = gameObject.GetComponent<Cell>();
        Character character = battleSystem.State is PlayerTurn ? battleSystem.PlayerController.CurrentPlayerCharacter : battleSystem.EnemyController.CurrentEnemyCharacter;
        Vector2 pos = character.PositionOnField;

        character.Move(0, currentCell.transform, moveTime);

        OnCardUse?.Invoke();
    }

}
