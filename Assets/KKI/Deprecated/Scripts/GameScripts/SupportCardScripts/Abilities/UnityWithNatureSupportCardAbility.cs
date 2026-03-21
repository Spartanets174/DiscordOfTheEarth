using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class UnityWithNatureSupportCardAbility : BaseSupportÑardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private Character character;
    private UnityWithNatureSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupportÑardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (UnityWithNatureSupportCardAbilityData)baseAbilityData;
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

        character.HealMoreThenMax(abilityData.healAmount, abilityData.supportÑardAbilityName);
        character.PhysDefence += abilityData.physDefence;
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
        character.PhysDefence -= abilityData.physDefence;
        bool isDeath = character.Damage(abilityData.healAmount, abilityData.supportÑardAbilityName);

        if (isDeath)
        {
            string characterType = "";
            Color characterColor;
            if (character is PlayerCharacter)
            {
                characterType = "ñîşçíûé";
                characterColor = battleSystem.playerTextColor;
            }
            else
            {
                characterType = "âğàæåñêèé";
                characterColor = battleSystem.enemyTextColor;

            }
            battleSystem.gameLogCurrentText.Value = $"İôôåêò äîïîëíèòåëüíîãî çäîğîâüÿ îò êàğòû \"Åäèíñòâî ñ ïğèğîäîé 2\" çàêàí÷èâàåòñÿ, {characterType} ïåğñîíàæ <color=#{characterColor.ToHexString()}>{character.CharacterName}</color> ïîãèáàåò";
            GameObject.Destroy(character.gameObject);
        }
        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class UnityWithNatureSupportCardAbilityData : BaseSupportÑardAbilityData
{
    public string selectCardText;

    public float physDefence;

    public int healAmount;

    public int turnCount;

    [Header("Íå òğîãàòü!")]
    public bool isBuff;
}