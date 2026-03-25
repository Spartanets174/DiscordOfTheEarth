using System;
using UnityEngine.TextCore.Text;

[Serializable]
public class CleansingSupportCardAbility : BaseSupport—ardAbility
{
    private CleansingSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        abilityData = (CleansingSupportCardAbilityData)baseAbilityData;
        this.battleSystem = battleSystem;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectCardText, battleSystem));
        SetSelectCharacterBehaviour(new EmptySelectCharacterBehaviour(""));

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
    }
    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += SelectCharacter;
            }
        }

    }
    private void OnSelectCharacter()
    {
        if (battleSystem.State is PlayerTurn)
        {
            battleSystem.PlayerController.CurrentPlayerCharacter.RemoveDebuffs();
            battleSystem.PlayerController.CurrentPlayerCharacter.InstantiateEffectOnCharacter(abilityData.effect);

        }
        else
        {
            battleSystem.EnemyController.CurrentEnemyCharacter.RemoveDebuffs();
            battleSystem.EnemyController.CurrentEnemyCharacter.InstantiateEffectOnCharacter(abilityData.effect);
        }
        battleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= SelectCharacter;
        });

        UseCard(null);
    }

    private void OnCancelSelection()
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= SelectCharacter;
        }
    }

}
[Serializable]
public class CleansingSupportCardAbilityData : BaseSupport—ardAbilityData
{
    public string selectCardText;
}