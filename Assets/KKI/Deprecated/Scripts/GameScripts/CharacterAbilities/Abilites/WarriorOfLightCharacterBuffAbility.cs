using System;

[Serializable]
public class WarriorOfLightCharacterBuffAbility : BaseCharacterAbility
{
    private WarriorOfLightCharacterBuffAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (WarriorOfLightCharacterBuffAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.OnDeath += OnDeath;




        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);
    }

    private void OnDeath(Character character)
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            if (playerCharacter.Class == Enums.Classes.Паладин)
            {
                playerCharacter.PhysAttack += abilityData.physDamageAmount;

                if (playerCharacter.Health == playerCharacter.MaxHealth)
                {
                    playerCharacter.MaxHealth += abilityData.healAmount;
                }
                playerCharacter.Heal(abilityData.healAmount, abilityOwner.CharacterName);
            }
        }
    }
}
[Serializable]
public class WarriorOfLightCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public float healAmount;

    public float physDamageAmount;

}