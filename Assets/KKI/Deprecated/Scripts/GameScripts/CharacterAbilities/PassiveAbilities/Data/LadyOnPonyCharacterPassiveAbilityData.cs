using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LadyOnPonyCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float damageIncrease;

    public Enums.Classes classToIncreaseDamage = new();
}
