using TypeReferences;
using UnityEngine;

public class BaseSupportСardAbilityData
{
    [Inherits(typeof(BaseSupportСardAbility), ShortName = true)]
    public TypeReference SupportСardAbility;

    public string supportСardAbilityName;

    public EffectData effect;
}
