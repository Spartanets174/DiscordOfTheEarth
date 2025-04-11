using System;

[Serializable]
public class ConnectionData
{
    public string ip = "127.0.0.1";
    public string port = "3306";
    public string uid = "root";
    public string pwd = "12345";
    public string database = "gamedb";

    public ConnectionData()
    {

    }

    public ConnectionData(string ip, string port, string uid, string pwd, string database)
    {
        this.ip = ip;
        this.port = port;
        this.uid = uid;
        this.pwd = pwd;
        this.database = database;
    }
}




