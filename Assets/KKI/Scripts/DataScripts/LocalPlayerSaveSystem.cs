using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;
public static class LocalPlayerSaveSystem
{
    private static string path = Application.dataPath + "/LocalSaves";
    private static string fileName = "PlayerSavedData.dote";
    private static string keyWord = "20042004s";
    public static void SavePlayerData(PlayerData playerData)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        PlayerDataSave playerDataSave = PlayerDataToSave(playerData);
        string json = EncryptOrDecryptString(JsonConvert.SerializeObject(playerDataSave));
        File.WriteAllText(path + "/" + fileName, json);
    }

    public static PlayerDataSave LoadPlayerData()
    {
        if (File.Exists(path + "/" + fileName))
        {
            string json = EncryptOrDecryptString(File.ReadAllText(path + "/" + fileName));
            return JsonConvert.DeserializeObject<PlayerDataSave>(json);
        }
        return null;
    }

    public static void DeletePlayer()
    {
        if (File.Exists(path + "/" + fileName))
        {
            File.Delete(path + "/" + fileName);
        }
    }

    private static string EncryptOrDecryptString(string data)
    {
        string result = "";

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }

        return result;
    }

    private static PlayerDataSave PlayerDataToSave(PlayerData playerData)
    {
        PlayerDataSave playerDataSave = new();
        playerDataSave.money = playerData.money;
        playerDataSave.Name = playerData.Name;
        playerDataSave.Password = playerData.Password;
        playerDataSave.PlayerId = playerData.PlayerId;

        foreach (var characterCard in playerData.allShopCharCards)
        {
            playerDataSave.allShopCharCards.Add(characterCard.id);
        }
        foreach (var cardSuppor in playerData.allShopSupportCards)
        {
            playerDataSave.allShopSupportCards.Add(cardSuppor.id);
        }
        foreach (var characterCard in playerData.allUserCharCards)
        {
            playerDataSave.allUserCharCards.Add(characterCard.id);
        }
        foreach (var cardSuppor in playerData.allUserSupportCards)
        {
            playerDataSave.allUserSupportCards.Add(cardSuppor.id);
        }
        foreach (var characterCard in playerData.deckUserCharCards)
        {
            playerDataSave.deckUserCharCards.Add(characterCard.id);
        }
        foreach (var cardSuppor in playerData.deckUserSupportCards)
        {
            playerDataSave.deckUserSupportCards.Add(cardSuppor.id);
        }

        return playerDataSave;
    }

}
