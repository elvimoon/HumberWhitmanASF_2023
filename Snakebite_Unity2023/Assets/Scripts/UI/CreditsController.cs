using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class CreditsController : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public string mainMenuScene;
    public void MainMenu()
    {
        StartCoroutine(LoadLevel(mainMenuScene));
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
