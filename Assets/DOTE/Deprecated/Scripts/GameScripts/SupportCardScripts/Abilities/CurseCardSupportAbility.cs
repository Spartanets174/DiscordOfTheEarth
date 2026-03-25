using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class CurseCardSupportAbility : BaseSupport—ardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    private List<EnemyCharacter> enemyCharacters;
    private List<PlayerCharacter> playerCharacters;

    private List<GameObject> effects = new();
    private CurseCardSupportAbilityData abilityData;
    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (CurseCardSupportAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("»ÒÔÓÎ¸ÁÛÈÚÂ Í‡ÚÛ"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        battleSystem.PointsOfAction.Value += abilityData.pointsOfAction;

        if (battleSystem.State is PlayerTurn)
        {
            enemyCharacters = battleSystem.EnemyController.EnemyCharObjects.Where(x => x.Class == Enums.Classes.œ‡Î‡‰ËÌ).ToList();
            foreach (var enemyCharacter in enemyCharacters)
            {
                enemyCharacter.IsFreezed = true;
                effects.Add(enemyCharacter.InstantiateEffectOnCharacter(abilityData.effect));
            }
        }
        else
        {
            playerCharacters = battleSystem.PlayerController.PlayerCharactersObjects.Where(x => x.Class == Enums.Classes.œ‡Î‡‰ËÌ).ToList();
            foreach (var playerCharacter in playerCharacters)
            {
                playerCharacter.IsFreezed = true;
                effects.Add(playerCharacter.InstantiateEffectOnCharacter(abilityData.effect));
            }
        }

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(null);
    }

    public void ReturnToNormal()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in playerCharacters)
            {
                playerCharacter.IsFreezed = false;
            }
        }
        else
        {
            foreach (var enemyCharacter in enemyCharacters)
            {
                enemyCharacter.IsFreezed = false;
            }

        }
        foreach (var item in effects)
        {
            Destroy(item);
        }
        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class CurseCardSupportAbilityData : BaseSupport—ardAbilityData
{
    public int pointsOfAction;

    public int turnCount;

    [Header("ÕÂ ÚÓ„‡Ú¸!")]
    public bool isBuff;
}