using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalPlayerRepository : APlayerRepository
{
    private PlayerDataSave playerDataSave;
    private void Awake()
    {
        OpenCon();
    }
    public override void CloseCon()
    {
        m_isConnected = false;
        Debug.Log("closed");
    }
    public override void OpenCon()
    {
        m_isConnected = true;
        playerDataSave = LocalPlayerSaveSystem.LoadPlayerData();
        if (playerDataSave == null)
        {
            playerDataSave = new();
        }
    }

    public override void SavePlayer()
    {
        LocalPlayerSaveSystem.SavePlayerData(playerData);
    }

    public override void InsertIntoCardsSupport(PlayerData playerData)
    {

    }

    public override void InsertToCardDeck(PlayerData playerData)
    {

    }

    public override void InsertToCardsOwn(PlayerData playerData)
    {

    }

    public override void InsertToCardsShop(PlayerData playerData)
    {

    }

    public override void InsertToCardsShopStart(PlayerData playerData)
    {
        for (int i = 0; i < playerData.allCharCards.Count; i++)
        {
            if (!playerData.startUserCharCards.Contains(playerData.allCharCards[i]))
            {
                playerDataSave.allShopCharCards.Add(playerData.allCharCards[i].id);
            }
        }
    }

    public override void InsertToCardsSupportShopStart(PlayerData playerData)
    {
        for (int i = 0; i < playerData.allSupportCards.Count; i++)
        {
            if (!playerData.startUserSupportCards.Contains(playerData.allSupportCards[i]))
            {
                playerDataSave.allShopSupportCards.Add(playerData.allSupportCards[i].id);
            }
        }
    }

    public override void InsertToOwnCardStart(PlayerData playerData)
    {
        for (int i = 0; i < playerData.startUserCharCards.Count; i++)
        {
            playerDataSave.allUserCharCards.Add(playerData.startUserCharCards[i].id);
        }
    }

    public override void InsertToOwnCardsSupportStart(PlayerData playerData)
    {
        for (int i = 0; i < playerData.startUserSupportCards.Count; i++)
        {
            playerDataSave.allUserSupportCards.Add(playerData.startUserSupportCards[i].id);
        }
    }
    public override void InsertToCardsSupportDeck(PlayerData playerData)
    {

    }

    public override void InsertToCardsSupportOwn(PlayerData playerData)
    {

    }

    public override void InsertToCardsSupportShop(PlayerData playerData)
    {

    }




    public override bool InsertToPlayers(string Name, string password, int balance)
    {
        return true;
    }

    public override void IsertIntoChars(PlayerData playerData)
    {

    }

    public override bool IsPlayerExits(string nick, string password)
    {
        return true;
    }


    public override void RemoveCardsDeck(PlayerData playerData)
    {

    }

    public override void RemoveCardsOwn(PlayerData playerData)
    {

    }

    public override void RemoveCardsShop(PlayerData playerData)
    {

    }

    public override void RemoveCardsSupportDeck(PlayerData playerData)
    {

    }

    public override void RemoveCardsSupportOwn(PlayerData playerData)
    {

    }

    public override void RemoveCardsSupportShop(PlayerData playerData)
    {

    }

    public override int SelectBalancePlayer(PlayerData playerData)
    {
        return playerData.money;
    }

    public override List<int> SelectFromCardsShop(PlayerData playerData)
    {
        return playerDataSave.allShopCharCards;
    }

    public override void SelectFromCardsSupport()
    {

    }

    public override List<int> SelectFromCardsSupportShop(PlayerData playerData)
    {
        return playerDataSave.allShopSupportCards;
    }

    public override void SelectFromChars()
    {

    }

    public override List<int> SelectFromDeckCards(PlayerData playerData)
    {
        return playerDataSave.deckUserCharCards;
    }

    public override List<int> SelectFromDeckCardsSupport(PlayerData playerData)
    {
        return playerDataSave.deckUserSupportCards;
    }

    public override List<int> SelectFromOwnCards(PlayerData playerData)
    {
        return playerDataSave.allUserCharCards;
    }

    public override List<int> SelectFromOwnCardsSupport(PlayerData playerData)
    {
        return playerDataSave.allUserSupportCards;
    }

    public override List<string> SelectFromPlayers()
    {
        return new List<string>();
    }

    public override int SelectIdPlayer(string playerName)
    {
        return playerData.PlayerId;
    }

    public override int UpdatePlayerBalance(PlayerData playerData)
    {
        return playerData.money;
    }
}
