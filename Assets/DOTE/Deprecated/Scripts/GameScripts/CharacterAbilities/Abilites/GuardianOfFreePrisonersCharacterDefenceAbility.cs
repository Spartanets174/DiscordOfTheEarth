using System;
using UnityEngine;

[Serializable]
public class GuardianOfFreePrisonersCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private GuardianOfFreePrisonersCharacterDefenceAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (GuardianOfFreePrisonersCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var character in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                if (character.Race == Enums.Races.МагическиеСущества)
                {
                    character.PhysDefence += abilityData.physDefenceAmount;
                    character.MagDefence += abilityData.magDefenceAmount;
                    character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

                }
            }
        }
        else
        {
            foreach (var character in battleSystem.EnemyController.EnemyCharObjects)
            {
                if (character.Race == Enums.Races.МагическиеСущества)
                {
                    character.PhysDefence += abilityData.physDefenceAmount;
                    character.MagDefence += abilityData.magDefenceAmount;
                    character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

                }
            }
        }
        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);

    }

    public void ReturnToNormal()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var character in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                if (character.Race == Enums.Races.МагическиеСущества)
                {
                    character.PhysDefence -= abilityData.physDefenceAmount;
                    character.MagDefence -= abilityData.magDefenceAmount;
                }
            }
        }
        else
        {
            foreach (var character in battleSystem.EnemyController.EnemyCharObjects)
            {
                if (character.Race == Enums.Races.МагическиеСущества)
                {
                    character.PhysDefence -= abilityData.physDefenceAmount;
                    character.MagDefence -= abilityData.magDefenceAmount;
                }
            }
        }

        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class GuardianOfFreePrisonersCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public float magDefenceAmount;

    public float physDefenceAmount;

    public int turnCount;

    [Header("Не трогать!!!")]
    public bool isBuff;
}