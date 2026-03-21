using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaidenOnUnicornCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{  
    [Range(0, 3)]
    public float physDamageIgnore;

    public List<Enums.Races> racesToIgnoreDamage = new();
}
