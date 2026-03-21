using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;

public class BasePassiveCharacterAbilityData
{
    [Inherits(typeof(BasePassiveCharacterAbility), ShortName = true)]
    public TypeReference passiveCharacterAbility;

    public string abilityName;
}
