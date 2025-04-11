using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSave 
{
    public List<int> allShopCharCards = new();
    public List<int> allShopSupportCards= new ();

    public List<int> allUserCharCards = new();
    public List<int> allUserSupportCards = new();

    public List<int> deckUserCharCards = new();
    public List<int> deckUserSupportCards = new();

    public int money;
    public string Name;
    public string Password;
    public int PlayerId;
}
