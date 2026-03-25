using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EternalGuardianCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0,1)]
    public float damageIgnorePercent;
}
