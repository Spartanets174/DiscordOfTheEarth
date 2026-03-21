using System;
using UnityEngine;

[Serializable]
public class ArtOfSurviveSupportCardAbility : BaseSupport—ardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private Character character;
    private ArtOfSurviveSupportCardAbilityData abilityData;

    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (ArtOfSurviveSupportCardAbilityData)baseAbilityData;
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
            character = battleSystem.PlayerController.CurrentPlayerCharacter;
        }
        else
        {
            character = battleSystem.EnemyController.CurrentEnemyCharacter;
        }
        character.InstantiateEffectOnCharacter(abilityData.effect);

        character.ChanceToAvoidDamage += abilityData.chanceToAvoidDamage;

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

    public void ReturnToNormal()
    {
        character.ChanceToAvoidDamage -= abilityData.chanceToAvoidDamage;

        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class ArtOfSurviveSupportCardAbilityData : BaseSupport—ardAbilityData
{
    public string selectCardText;

    public float chanceToAvoidDamage;

    public int turnCount;

    [Header("ÕÂ ÚÓ„‡Ú¸!")]
    public bool isBuff;
}