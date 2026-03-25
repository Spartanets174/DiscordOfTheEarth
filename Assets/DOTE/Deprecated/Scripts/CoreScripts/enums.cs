using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums 
{
    public enum Races
    {
        Все,
        Люди,
        Гномы,
        Эльфы,
        ТёмныеЭльфы,
        МагическиеСущества
    }
    public enum Classes
    {
        Все,
        Паладин,
        Лучник,
        Кавалерия,
        Маг
    }
    public enum Rarity
    {
        Все,
        Обычная,
        Мифическая
    }
    public enum TypeOfSupport
    {
        Все,
        атакующая,
        защитная,
        мобильность
    }

    public enum TypeOfAbility
    {
        attack,
        defence,
        buff
    }

    public enum Directions
    {
        top,
        bottom,
        left,
        right,
        all,
        middle
    }
}
