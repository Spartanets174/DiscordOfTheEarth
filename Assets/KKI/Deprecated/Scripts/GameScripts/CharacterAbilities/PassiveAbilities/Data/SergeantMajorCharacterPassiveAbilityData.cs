using System;

[Serializable]
public class SergeantMajorCharacterPassiveAbilityData : BasePassiveCharacterAbilityData
{
    public int range;
    public int physDefenceAmount;
    public int magDefenceAmount;
    public Enums.Races raceToBuff;
}
