using UnityEngine;

public static class ConnectionInfo
{
    public static ConnectionData ConnectionData { get; private set; } = new ConnectionData();

    public static void ChangeConncetionInfo(ConnectionData connectionData)
    {
        ConnectionData = connectionData;
        PlayerPrefs.SetString("ip", ConnectionData.ip);
        PlayerPrefs.SetString("port", ConnectionData.port);
        PlayerPrefs.SetString("uid", ConnectionData.uid);
        PlayerPrefs.SetString("pwd", ConnectionData.pwd);
        PlayerPrefs.SetString("database", ConnectionData.database);

        Debug.Log("ip: "+PlayerPrefs.GetString("ip"));
        Debug.Log("port: " + PlayerPrefs.GetString("port"));
        Debug.Log("uid: " + PlayerPrefs.GetString("uid"));
        Debug.Log("pwd: " + PlayerPrefs.GetString("pwd"));
        Debug.Log("database " + PlayerPrefs.GetString("database"));
    }
}




