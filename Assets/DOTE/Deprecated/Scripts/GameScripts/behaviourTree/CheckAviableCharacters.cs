using NodeCanvas.Framework;

public class CheckAviableCharacters : ConditionTask
{
    public BBParameter<BattleSystem> battleSystem;
    public BBParameter<EnemyController> enemyController;
    protected override bool OnCheck()
    {
        int count = 0;
        for (int i = 0; i < enemyController.value.EnemyCharObjects.Count; i++)
        {
            if (enemyController.value.EnemyCharObjects[i].Speed == 0 && enemyController.value.EnemyCharObjects[i].IsAttackedOnTheMove)
            {
                count++;
            }
        }
        if (count == enemyController.value.EnemyCharObjects.Count)
        {
            battleSystem.value.SetPlayerTurn();
            return false;
        }
        else
        {
            return true;
        }
    }
}
