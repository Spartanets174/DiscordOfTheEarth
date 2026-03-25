using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class YouWontCatchSupportCardAbility : BaseSupport—ardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private Character character;
    private float physAmount;
    private float magAmount;
    private YouWontCatchSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (YouWontCatchSupportCardAbilityData)baseAbilityData;
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

        physAmount = (character.PhysAttack * (1+ abilityData.increacePercent)) - character.PhysAttack;
        magAmount = (character.MagAttack * (1 + abilityData.increacePercent)) - character.MagAttack;


        character.MagAttack += magAmount;
        character.PhysAttack += physAmount;
        character.InstantiateEffectOnCharacter(abilityData.effect);

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
        character.MagAttack -= magAmount;
        character.PhysAttack -= physAmount;


        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class YouWontCatchSupportCardAbilityData : BaseSupport—ardAbilityData
{
    public string selectCardText;

    public float increacePercent;

    public int turnCount;

    [Header("ÕÂ ÚÓ„‡Ú¸!")]
    public bool isBuff;
}