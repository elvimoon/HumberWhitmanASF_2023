using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string gameScene;
    public string creditsScene;
    public void NewGame()
    {
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }
    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsScene, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
