using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarriorOfLightCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private WarriorOfLightCharacterPassiveAbilityData abilityData;

    private Character buffedCharacter;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (WarriorOfLightCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);
        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        if (abilityOwner is PlayerCharacter)
        {
            List<PlayerCharacter> characters = battleSystem.PlayerController.PlayerCharactersObjects.Where(x=>x.MagDefence <= abilityData.selectMagDefenceAmount && x != abilityOwner).ToList();
            buffedCharacter = characters[UnityEngine.Random.Range(0, characters.Count)];
        }
        else
        {
            List<EnemyCharacter> characters = battleSystem.EnemyController.EnemyCharObjects.Where(x => x.MagDefence <= abilityData.selectMagDefenceAmount && x != abilityOwner).ToList();
            buffedCharacter = characters[UnityEngine.Random.Range(0, characters.Count)];
        }
        buffedCharacter.MagDefence += abilityData.buffMagDefenceAmount;
    }

    public override void AbilityEnd(Character character)
    {
        buffedCharacter.MagDefence -= abilityData.buffMagDefenceAmount;
        abilityOwner.OnDeath -= AbilityEnd;
    }
}