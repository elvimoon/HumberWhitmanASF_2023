using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public string gameScene;
    public string creditsScene;
    public void NewGame()
    {
        StartCoroutine(LoadLevel(gameScene));
    }
    public void OpenCredits()
    {
        StartCoroutine(LoadLevel(creditsScene));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
