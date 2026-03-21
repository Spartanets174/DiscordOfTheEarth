using System;
using UnityEngine;

[Serializable]
public class SelfTaughtedCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float damageIncrease;

    public Enums.Classes classToIncreaseDamage = new();
}
