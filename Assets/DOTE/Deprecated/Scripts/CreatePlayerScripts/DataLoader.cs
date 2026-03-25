using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataLoader : MonoBehaviour, ILoadable
{
    [SerializeField] DbManager DB;

    private PlayerData playerData;

    public event Action<string> OnPlayerDataRecieved;
    public event Action<bool> OnPlayerDataChecked;
    public void Init()
    {
        playerData = DB.PlayerData;
        OnPlayerDataRecieved += LoadMenuScene;
    }
    private void OnDestroy()
    {
        OnPlayerDataRecieved -= LoadMenuScene;
    }

    public void CheckPlayerData()
    {
        string[] creditials = SaveSystem.LoadPlayer().Split(".");
        if (creditials.Length == 0)
        {
            OnPlayerDataChecked?.Invoke(false);
        }

        if (creditials[0] != "" && creditials[1] != "")
        {
           StartCoroutine(GetPlayerData(creditials[0], creditials[1]));
            OnPlayerDataChecked?.Invoke(true);
        }
        else
        {
            OnPlayerDataChecked?.Invoke(false);
        }
    }

    private void LoadMenuScene(string connectionAnwser)
    {
        if (connectionAnwser == "loginned")
        {
            SceneController.ToMenu();
        }
    }

    public bool IsNicknameInBase(string Nick)
    {
        bool hasName = false;
        List<string> nickList = DB.SelectFromPlayers();
        for (int i = 0; i < nickList.Count; i++)
        {
            if (nickList[i] == Nick)
            {
                hasName = true;
            }
        }
        if (nickList.Count == 0 || !hasName)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator CreateNewPlayer(string Nick, string password)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (!DB.IsConnected) { OnPlayerDataRecieved?.Invoke("notConnected"); yield break; };
        if (!DB.InsertToPlayers(Nick, password, 10000)) { OnPlayerDataRecieved?.Invoke("wrongCreditials"); yield break; };

        
        playerData.Name = Nick;
        playerData.Password = password;
        SaveSystem.SavePlayer(playerData.Name, playerData.Password);
        DB.SelectFromChars();
        DB.SelectFromCardsSupport();
        int id = DB.SelectIdPlayer(playerData.Name);

        playerData.money = 10000;
        playerData.PlayerId = id;
       

        DB.InsertToCardsShopStart(playerData);
        DB.InsertToCardsSupportShopStart(playerData);
        DB.InsertToOwnCardStart(playerData);
        DB.InsertToOwnCardsSupportStart(playerData);

        List<int> CardOfShopPlayer = DB.SelectFromCardsShop(playerData);
        List<int> CardSupportOfShopPlayer = DB.SelectFromCardsSupportShop(playerData);
        List<int> OwnedCardOfPlayer = DB.SelectFromOwnCards(playerData);
        List<int> OwnedCardSupportOfPlayer = DB.SelectFromOwnCardsSupport(playerData);

        playerData.allShopCharCards.Clear();
        playerData.allShopSupportCards.Clear();
        playerData.allUserCharCards.Clear();
        playerData.allUserSupportCards.Clear();
        playerData.deckUserCharCards.Clear();
        playerData.deckUserSupportCards.Clear();

        List<CharacterCard> CharacterCards = Resources.LoadAll<CharacterCard>($"cards/characters").OrderBy(x => x.cardName).ToList();
        List<CardSupport> CardsSupport = Resources.LoadAll<CardSupport>($"cards/support").OrderBy(x => x.cardName).ToList();

        for (int i = 0; i < CardOfShopPlayer.Count; i++)
        {
            CharacterCard card = CharacterCards.Where(x => x.id == CardOfShopPlayer[i]).FirstOrDefault();
            playerData.allShopCharCards.Add(card);
        }
        
        for (int i = 0; i < CardSupportOfShopPlayer.Count; i++)
        {
            CardSupport CardSupport = CardsSupport.Where(x => x.id == CardSupportOfShopPlayer[i]).FirstOrDefault();
            playerData.allShopSupportCards.Add(CardSupport);
        }

        
        for (int i = 0; i < OwnedCardOfPlayer.Count; i++)
        {
            CharacterCard card = CharacterCards.Where(x => x.id == OwnedCardOfPlayer[i]).FirstOrDefault();
            playerData.allUserCharCards.Add(card);
        }
        
        for (int i = 0; i < OwnedCardSupportOfPlayer.Count; i++)
        {
            CardSupport CardSupport = CardsSupport.Where(x => x.id == OwnedCardSupportOfPlayer[i]).FirstOrDefault();
            playerData.allUserSupportCards.Add(CardSupport);
        }

        OnPlayerDataRecieved?.Invoke("loginned");
        yield break;
    }

    public IEnumerator GetPlayerData(string Nick, string password)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (!DB.IsConnected) { OnPlayerDataRecieved?.Invoke("notConnected"); yield break; };
        if (!DB.IsPlayerExits(Nick, password)) { OnPlayerDataRecieved?.Invoke("wrongCreditials"); yield break; }; 

        playerData.Name = Nick;
        playerData.Password = password;
        SaveSystem.SavePlayer(playerData.Name, playerData.Password);

        playerData.PlayerId = DB.SelectIdPlayer(playerData.Name);
        playerData.money = DB.SelectBalancePlayer(playerData);
        DB.SelectFromChars();
        DB.SelectFromCardsSupport();

        List<int> CardOfShopPlayer = DB.SelectFromCardsShop(playerData);
        List<int> CardSupportOfShopPlayer = DB.SelectFromCardsSupportShop(playerData);
        List<int> OwnedCardOfPlayer = DB.SelectFromOwnCards(playerData);
        List<int> OwnedCardSupportOfPlayer = DB.SelectFromOwnCardsSupport(playerData);
        List<int> DeckCardOfPlayer = DB.SelectFromDeckCards(playerData);
        List<int> DeckCardSupportOfPlayer = DB.SelectFromDeckCardsSupport(playerData);

        List<CharacterCard> CharacterCards = Resources.LoadAll<CharacterCard>($"cards/characters").OrderBy(x => x.cardName).ToList();
        List<CardSupport> CardsSupport = Resources.LoadAll<CardSupport>($"cards/support").OrderBy(x => x.cardName).ToList();

        playerData.allShopCharCards.Clear();
        playerData.allShopSupportCards.Clear();
        playerData.allUserCharCards.Clear();
        playerData.allUserSupportCards.Clear();
        playerData.deckUserCharCards.Clear();
        playerData.deckUserSupportCards.Clear();

        for (int i = 0; i < CardOfShopPlayer.Count; i++)
        {
            CharacterCard card = CharacterCards.Where(x => x.id == CardOfShopPlayer[i]).FirstOrDefault();
            playerData.allShopCharCards.Add(card);
        }
       
        for (int i = 0; i < CardSupportOfShopPlayer.Count; i++)
        {
            CardSupport CardSupport = CardsSupport.Where(x => x.id == CardSupportOfShopPlayer[i]).FirstOrDefault();
            playerData.allShopSupportCards.Add(CardSupport);
        }
        
        for (int i = 0; i < OwnedCardOfPlayer.Count; i++)
        {
            CharacterCard card = CharacterCards.Where(x => x.id == OwnedCardOfPlayer[i]).FirstOrDefault();
            playerData.allUserCharCards.Add(card);
        }
        
        for (int i = 0; i < OwnedCardSupportOfPlayer.Count; i++)
        {
            CardSupport CardSupport = CardsSupport.Where(x => x.id == OwnedCardSupportOfPlayer[i]).FirstOrDefault();
            playerData.allUserSupportCards.Add(CardSupport);
        }
       
        for (int i = 0; i < DeckCardOfPlayer.Count; i++)
        {
            CharacterCard card = CharacterCards.Where(x => x.id == DeckCardOfPlayer[i]).FirstOrDefault();
            playerData.deckUserCharCards.Add(card);
        }
        
        for (int i = 0; i < DeckCardSupportOfPlayer.Count; i++)
        {
            CardSupport CardSupport = CardsSupport.Where(x => x.id == DeckCardSupportOfPlayer[i]).FirstOrDefault();
            playerData.deckUserSupportCards.Add(CardSupport);
        }

        OnPlayerDataRecieved?.Invoke("loginned");
        yield break;
    }
}