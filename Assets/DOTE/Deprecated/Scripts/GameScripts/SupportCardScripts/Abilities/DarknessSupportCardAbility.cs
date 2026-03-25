using System;
using UnityEngine;

[Serializable]
public class DarknessSupportCardAbility : BaseSupportÑardAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private Character character;

    private Character enemyCharacter;

    private DarknessSupportCardAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, BaseSupportÑardAbilityData baseAbilityData)
    {
        this.battleSystem = battleSystem;
        abilityData = (DarknessSupportCardAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectCardText, battleSystem));
        SetSecondCardSelectBehaviour(new SelectAllEnemyUnitsBehaviour(abilityData.selectCharacterText, battleSystem));
        SetSelectCharacterBehaviour(new SetCurrentEnemyCharacterBehaviour("", battleSystem));

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSecondSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_cardSecondSelectBehaviour.OnSelected += OnSecondSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += SelectSecondCharacterInvoke;
            }

        }
    }

    private void SelectSecondCharacterInvoke(GameObject gameObject)
    {
        if (battleSystem.State is PlayerTurn)
        {
            character = battleSystem.PlayerController.CurrentPlayerCharacter;
        }
        else
        {
            character = battleSystem.EnemyController.CurrentEnemyCharacter;
        }

        battleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= SelectSecondCharacterInvoke;
        });

        SelectSecondCard();
    }



    private void OnSecondSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            battleSystem.EnemyController.SetEnemiesState(true, (x) =>
            {
                x.OnClick += SelectCharacter;
            });
        }
    }

    private void OnSelectCharacter()
    {
        if (battleSystem.State is PlayerTurn)
        {
            enemyCharacter = battleSystem.EnemyController.CurrentEnemyCharacter;
        }
        else
        {
            enemyCharacter = battleSystem.PlayerController.CurrentPlayerCharacter;
        }

        foreach (var enemyCharacter in battleSystem.EnemyController.EnemyCharObjects)
        {
            enemyCharacter.OnClick -= SelectCharacter;
        }

        character.MagAttack += abilityData.characterMagAttack;
        character.InstantiateEffectOnCharacter(abilityData.effect);

        enemyCharacter.MagDefence -= abilityData.enemyCharacterMagDefence;
        enemyCharacter.InstantiateEffectOnCharacter(abilityData.secondEffect);


        UseCard(null);
    }

    private void OnCancelSelection()
    {
        battleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= SelectSecondCharacterInvoke;
        });

        battleSystem.EnemyController.SetEnemiesChosenState(false, x => { x.OnClick -= SelectCharacter; });
    }

    public void ReturnToNormal()
    {
        character.MagAttack -= abilityData.characterMagAttack;
        enemyCharacter.MagDefence += abilityData.enemyCharacterMagDefence;

        OnReturnToNormal?.Invoke(this);
    }
}
[Serializable]
public class DarknessSupportCardAbilityData : BaseSupportÑardAbilityData
{
    public EffectData secondEffect;

    public string selectCardText;

    public string selectCharacterText;

    public float characterMagAttack;

    public float enemyCharacterMagDefence;

    public int turnCount;

    [Header("Íå òğîãàòü!")]
    public bool isBuff;
}