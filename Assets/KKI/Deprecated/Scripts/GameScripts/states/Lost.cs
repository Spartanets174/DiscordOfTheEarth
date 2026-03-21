using System.Collections;

public class Lost: State
{ 
    public Lost(BattleSystem battleSystem): base(battleSystem)
    {
    }
    public override IEnumerator Start()
    {
        BattleSystem.PlayerController.PlayerDataController.Money += 500;
        yield break;
    }
}