using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerRepository: MonoBehaviour
{
    public static string connectionString;

    public static MySqlConnection con;

    [SerializeField] protected PlayerData playerData;

    public PlayerData PlayerData => playerData;

    protected bool m_isConnected;
    public bool IsConnected => m_isConnected;

    public abstract void OpenCon();
    public abstract void CloseCon();
    public abstract void SavePlayer();
    public abstract bool InsertToPlayers(string Name, string password, int balance);
    public abstract bool IsPlayerExits(string nick, string password);
    public abstract List<string> SelectFromPlayers();
    public abstract int UpdatePlayerBalance(PlayerData playerData);
    public abstract int SelectBalancePlayer(PlayerData playerData);
    public abstract int SelectIdPlayer(string playerName);
    public abstract void IsertIntoChars(PlayerData playerData);
    public abstract void SelectFromChars();
    public abstract void InsertIntoCardsSupport(PlayerData playerData);
    public abstract void SelectFromCardsSupport();
    public abstract void InsertToCardsSupportShop(PlayerData playerData);
    public abstract void RemoveCardsSupportShop(PlayerData playerData);
    public abstract void InsertToCardsSupportShopStart(PlayerData playerData);
    public abstract List<int> SelectFromCardsSupportShop(PlayerData playerData);
    public abstract void InsertToCardsShop(PlayerData playerData);
    public abstract void RemoveCardsShop(PlayerData playerData);
    public abstract void InsertToCardsShopStart(PlayerData playerData);
    public abstract List<int> SelectFromCardsShop(PlayerData playerData);
    public abstract void InsertToCardsSupportOwn(PlayerData playerData);
    public abstract void RemoveCardsSupportOwn(PlayerData playerData);
    public abstract void InsertToOwnCardsSupportStart(PlayerData playerData);
    public abstract List<int> SelectFromOwnCardsSupport(PlayerData playerData);
    public abstract void InsertToCardsOwn(PlayerData playerData);
    public abstract void RemoveCardsOwn(PlayerData playerData);
    public abstract void InsertToOwnCardStart(PlayerData playerData);
    public abstract List<int> SelectFromOwnCards(PlayerData playerData);
    public abstract void InsertToCardsSupportDeck(PlayerData playerData);
    public abstract void RemoveCardsSupportDeck(PlayerData playerData);
    public abstract List<int> SelectFromDeckCardsSupport(PlayerData playerData);
    public abstract void InsertToCardDeck(PlayerData playerData);
    public abstract void RemoveCardsDeck(PlayerData playerData);
    public abstract List<int> SelectFromDeckCards(PlayerData playerData);

    private void OnApplicationQuit()
    {
        SavePlayer();

        CloseCon();
    }
}
