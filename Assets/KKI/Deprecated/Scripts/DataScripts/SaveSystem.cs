using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem 
{
    private static string path= Application.dataPath + "/player.fun";
    public static void SavePlayer(string Name, string Password)
    {
        if (File.Exists(path)) return;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,  FileMode.Create);
        string nameToSave = Name + "." + Password;
        formatter.Serialize(stream, nameToSave);
        stream.Close();
    }

    public static string LoadPlayer()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            string stringToLoad = (string)formatter.Deserialize(stream);
            stream.Close();
            return stringToLoad;
        }
        else
        {        
            return "";
        }

    }

    public static void DeletePlayer()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
