using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController 
{
    public static void Exit()
    {
        Debug.Log("out");
        Application.Quit();
    }
    public static void ToGame()
    {
        SceneManager.LoadScene("Game");
    }
    public static void ToCreatePlayer()
    {
        SceneManager.LoadScene("CreatePlayer");
    }
    public static void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
