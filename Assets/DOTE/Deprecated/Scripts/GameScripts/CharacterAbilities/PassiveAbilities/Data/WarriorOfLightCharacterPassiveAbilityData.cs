using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WarriorOfLightCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    [Range(0, 3),Tooltip("Значение магической защиты, по которому производится выбор персонажа (если меньше, то включается в список для выбора)")]
    public float selectMagDefenceAmount;

    [Range(0, 3), Tooltip("Значение магической защиты, на которое усиливается случайно выбранный персонаж")]
    public float buffMagDefenceAmount;
}
