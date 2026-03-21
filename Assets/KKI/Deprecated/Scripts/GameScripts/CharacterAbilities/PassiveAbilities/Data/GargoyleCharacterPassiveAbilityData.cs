using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GargoyleCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0,3)]
    public float healthAmountToInvoke;
    [Range(0, 3)]
    public int physDefenceAmount;
    public int range;
    public int turnCount;    
    public Enums.Directions direction;

    [Header("Õ≈ “–Œ√¿“‹!!!")]
    public bool isBuff;
}
