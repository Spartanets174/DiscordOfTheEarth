using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OldMasterCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 3)]
    public float damageIncrease;

    public List<Enums.Races> racesToIncreaseDamage = new();
}
