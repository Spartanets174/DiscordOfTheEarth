using System;
using UnityEngine;

[Serializable]
public class VoidShooterCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float magDamageIncreaseAmount;
    [Range(0, 1)]
    public float physDamageDecreaseAmount;
}
