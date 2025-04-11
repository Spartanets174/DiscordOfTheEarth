using System.IO;
using UnityEngine;
public static class SaveSystem
{
    private static string path = Application.dataPath + "/PlayerCreditionals.dote";
    private static string keyWord = "20042004s";
    public static void SavePlayer(string Name, string Password)
    {
        if (File.Exists(path)) return;
        string nameToSave = EncryptOrDecryptString(Name + "." + Password);
        File.WriteAllText(path, nameToSave);
    }

    public static string LoadPlayer()
    {
        if (File.Exists(path))
        {
            string stringToLoad = EncryptOrDecryptString(File.ReadAllText(path));
            return stringToLoad;
        }
        else
        {
            return "";
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

    public static void DeletePlayer()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
