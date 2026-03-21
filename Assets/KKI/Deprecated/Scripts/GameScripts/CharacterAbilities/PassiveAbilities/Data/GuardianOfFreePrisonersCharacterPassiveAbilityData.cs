using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GuardianOfFreePrisonersCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float chance;
}
