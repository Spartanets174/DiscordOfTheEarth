using System;
using UnityEngine;


[Serializable]
public class SergeantMajorCharacterBuffAbility : BaseCharacterAbility
{
    private SergeantMajorCharacterBuffAbilityData abilityData;
    private Character character;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (SergeantMajorCharacterBuffAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectAbilityText, battleSystem));
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

        float chance = UnityEngine.Random.Range(0f, 1f);

        if (chance <= abilityData.chanceToIncreaseDamage)
        {
            character.PhysAttack += abilityData.physDamageAmount;
            character.InstantiateEffectOnCharacter(abilityData.useEffects["buffDamage"]);
        }
        else
        {
            character.PhysDefence += abilityData.physDefenceAmount;
            character.MagDefence += abilityData.magDefenceAmount;
            character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);
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
public class SergeantMajorCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float physDamageAmount;

    public float physDefenceAmount;

    public float magDefenceAmount;

    public float chanceToIncreaseDamage;
}