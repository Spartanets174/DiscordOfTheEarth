using System;
using UnityEngine;

[Serializable]
public class ProtectedDonkeyCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float chance;
    public int range;
    public Enums.Directions direction;
}
