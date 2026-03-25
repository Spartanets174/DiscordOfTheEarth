using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    [Space, Header("Info")]
    public string cardName;
    public string description;
    public Enums.Races race;
    public Sprite image;
    public Enums.Rarity rarity;
    public int Price;
    public int id;
}
