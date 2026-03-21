using System;
using UnityEngine.TextCore.Text;

[Serializable]
public class SergeantMajorCharacterDefenceAbility : BaseCharacterAbility
{
    private SergeantMajorCharacterDefenceAbilityData abilityData;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (SergeantMajorCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.MagDefence += abilityData.magDefenceAmount;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);

    }

}
[Serializable]
public class SergeantMajorCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public float magDefenceAmount;
}