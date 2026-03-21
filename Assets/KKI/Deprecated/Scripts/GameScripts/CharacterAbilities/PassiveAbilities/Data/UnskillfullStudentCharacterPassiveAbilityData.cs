using System;
using UnityEngine;

[Serializable]
public class UnskillfullStudentCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float chance;
}
