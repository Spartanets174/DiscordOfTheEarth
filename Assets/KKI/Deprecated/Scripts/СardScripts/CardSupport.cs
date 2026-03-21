using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Support", menuName = "Card support")]
public class CardSupport : Card
{
    public Enums.TypeOfSupport type;
    public string abilityText;

    [SerializeReference, SubclassSelector]
    public BaseSupport—ardAbilityData support—ardAbilityData;

}