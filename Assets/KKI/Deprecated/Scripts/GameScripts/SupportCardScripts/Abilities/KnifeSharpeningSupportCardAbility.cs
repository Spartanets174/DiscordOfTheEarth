using System;
using UnityEngine;

[Serializable]
public class KnifeSharpeningSupportCardAbility : BaseSupport—ardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private Character character;
    private KnifeSharpeningSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupport—ardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (KnifeSharpeningSupportCardAbilityData)baseAbilityData;
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

        character.CritChance += abilityData.critChance;
        character.CritNum += abilityData.critNum;
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
        character.CritChance -= abilityData.critChance;
        character.CritNum -= abilityData.critNum;

        OnReturnToNormal?.Invoke(this);
    }
}

[Serializable]
public class KnifeSharpeningSupportCardAbilityData : BaseSupport—ardAbilityData
{
    public string selectCardText;

    public float critChance;

    public float critNum;

    public int turnCount;

    [Header("ÕÂ ÚÓ„‡Ú¸!")]
    public bool isBuff;
}