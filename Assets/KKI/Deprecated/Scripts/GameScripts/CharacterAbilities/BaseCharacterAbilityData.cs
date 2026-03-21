using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;

public class BaseCharacterAbilityData
{
    [Inherits(typeof(BaseCharacterAbility), ShortName = true)]
    public TypeReference characterAbility;

    [Header("Ability effects")]
    [Tooltip("Не менять ключи!!!")]
    public EffectsDitionary useEffects ;
    public Sprite abilityImage;

    [Header("Ability settings")]
    public string abilityName;
}

[Serializable]
public class EffectsDitionary : SerializableDictionaryBase<string, EffectData> { }

[Serializable]
public class EffectData
{
    public GameObject effect;
    public AudioClip effectSound;
    public Enums.Directions spawnPoint;
}