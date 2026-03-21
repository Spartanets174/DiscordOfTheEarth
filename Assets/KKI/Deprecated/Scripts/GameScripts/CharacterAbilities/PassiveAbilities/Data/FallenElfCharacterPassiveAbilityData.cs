using System;
using UnityEngine;

[Serializable]
public class FallenElfCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float damageIncrease;

    public Enums.Races raceToIncreaseDamage = new();
}
