using System;
using UnityEngine;

[Serializable]
public class MagicToadCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 1)]
    public float damageIgnore;

    public Enums.Classes classToIgnoreDamage;
}
