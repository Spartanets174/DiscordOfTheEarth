using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DarkElfArcherCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float damageIncrease;

    public Enums.Classes classToIncreaseDamage = new();
}
