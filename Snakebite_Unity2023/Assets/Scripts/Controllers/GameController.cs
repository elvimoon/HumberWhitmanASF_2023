using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController Instance;

    public GameScene currentScene;
    public GameScene dScene;
    public GameScene rScene;
    public BottomBarController bottomBarController;
    public BackgroundController backgroundController;
    public ChooseController chooseController;
    public AudioController audioController;
    public GameObject resultsSign;
    public GameObject nextButton;
    public GameObject donateButton;
    public GameObject endButton;
    public Animator transition;
    public float transitionTime = 1f;
    public string creditsScene;
    private Vector2 resolution;
    public GameObject timeBar;
    public TextMeshProUGUI timeText;
    private int totalTime = 0;
    private bool isFirstEndSceen = true;
    public Queue<TextScene> endScenes = new Queue<TextScene>();

    private bool isNextButtonHidden = true;
    private bool isDonateButtonHidden = true;
    private State state = State.IDLE;

    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }

    void Awake() => Instance = this;

    void Start()
    {
        //endScenes.Enqueue(Resources.Load("Story/Scenes/Ending/Summary1") as TextScene);
        //endScenes.Enqueue(Resources.Load("Story/Scenes/Ending/Summary2") as TextScene);
        resolution = new Vector2(Screen.width, Screen.height);
        StartCoroutine(OnStart());
    }

    private IEnumerator OnStart()
    {
        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            backgroundController.SetImage(storyScene.background);
            yield return new WaitForSeconds(1.5f);
            bottomBarController.ClearText(storyScene);
            bottomBarController.Show();
            PlayAudio(storyScene.sentences[0]);
            yield return new WaitForSeconds(1f);
            bottomBarController.PlayScene(storyScene);
        }
    }

    //pressing spacebar or left mouse button will display the following sentence
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !(currentScene is DonateScene))
        {
            if(state == State.IDLE && bottomBarController.IsCompleted())
            {
                if (bottomBarController.IsLastSentence())
                {
                    if (currentScene is SummaryScene || currentScene is ResultScene)
                        PlayEndScenes();
                    else if ((currentScene as StoryScene).nextScene)
                        PlayScene((currentScene as StoryScene).nextScene);
                    else
                        PlayEndScenes();
                }

                else if (currentScene is StoryScene)
                {
                    bottomBarController.PlayNextSentence();
                    PlayAudio((currentScene as StoryScene)
                        .sentences[bottomBarController.GetSentenceIndex()]);
                }
            }
        }
        // Show button only after Text is completed
        //else if(bottomBarController.IsCompleted() && currentScene is SummaryScene && isNextButtonHidden)
        //{
        //    nextButton.GetComponent<Animator>().SetTrigger("Show");
        //    isNextButtonHidden = false;
        //}
        // Show buttons only after Text is completed
        else if (bottomBarController.IsCompleted() && currentScene is DonateScene && isDonateButtonHidden)
        {
            StartCoroutine(ShowDonateSceneButtons());
            isDonateButtonHidden = false;
        }
        if (currentScene is DonateScene || currentScene is SummaryScene)
        {
            if((Screen.width != resolution.x || Screen.height != resolution.y))
            {
                //bottomBarController.Resize(currentScene as TextScene);
                //resolution = new Vector2(Screen.width, Screen.height);
            }
        }
    }
    private IEnumerator ShowDonateSceneButtons()
    {
        yield return new WaitForSeconds(1f);
        donateButton.GetComponent<Animator>().SetTrigger("Show");
        yield return new WaitForSeconds(6f);
        endButton.GetComponent<Animator>().SetTrigger("Show");
    }

    public void PlayScene(GameScene scene)
    {
        bottomBarController.SetWaitingState();
        StartCoroutine(SwitchScene(scene));
    }

    public void PlayEndScenes()
    {
        if (isFirstEndSceen)
        {
            endScenes.Enqueue(rScene as TextScene);
            endScenes.Enqueue(dScene as TextScene);
            isFirstEndSceen = false;
        }
        bottomBarController.SetWaitingState();
        StartCoroutine(SwitchScene(endScenes.Dequeue()));
    }

    //function to create a smooth transition between scenes using IEnumerator to wait for end of the anims
    private IEnumerator SwitchScene(GameScene scene)
    {
        //at beginning of anim, panel will disappear, then bg will change, then panel will appear
        state = State.ANIMATE;
        if(currentScene is ResultScene) resultsSign.GetComponent<Animator>().SetTrigger("Hide");
        //if(currentScene is SummaryScene) nextButton.GetComponent<Animator>().SetTrigger("Hide");
        currentScene = scene;
        bottomBarController.Hide();
        yield return new WaitForSeconds(1f);

        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            PlayAudio(storyScene.sentences[0]);
            if(backgroundController.SwitchImage(storyScene.background))
                yield return new WaitForSeconds(1f);
            bottomBarController.ClearText(storyScene);
            bottomBarController.Show();
            yield return new WaitForSeconds(1f);
            bottomBarController.PlayScene(storyScene);

            totalTime += storyScene.time;
            timeText.text = "Time Since Bitten: " + totalTime.ToString();
            if (storyScene.isTimeVisible)
            {
                timeBar.SetActive(true);
            }

            state = State.IDLE;
        }

        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }

        else if (scene is SummaryScene)
        {
            SummaryScene summaryScene = scene as SummaryScene;
            PlayAudio(summaryScene.sentences[0]);
            if (backgroundController.SwitchImage(summaryScene.background))
                yield return new WaitForSeconds(1f);

            #region TextBarAlignment
            // Move TextBar to the middle of the screen and Resize it
            RectTransform rectTransform = bottomBarController.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
            rectTransform.anchorMax = new Vector2(1.0f, 0.5f);
            rectTransform.offsetMin = new Vector2(260f, 0f);
            rectTransform.offsetMax = new Vector2(-260f, 0f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 500f);
            //rectTransform.anchoredPosition -= new Vector2(0f, 50f);

            // Realign and resize the text
            bottomBarController.barText.GetComponent<RectTransform>().offsetMin = new Vector2(30f, 20f);
            bottomBarController.barText.GetComponent<RectTransform>().offsetMax = new Vector2(-30f, -20f);
            bottomBarController.barText.fontSize = 100;
            #endregion

            bottomBarController.ClearText(summaryScene);
            bottomBarController.Resize(summaryScene);
            bottomBarController.Show();
            yield return new WaitForSeconds(1f);
            bottomBarController.PlayScene(summaryScene as TextScene);
            state = State.IDLE;
        }

        else if (scene is ResultScene)
        {
            ResultScene resultScene = scene as ResultScene;
            PlayAudio(resultScene.sentences[0]);
            if (backgroundController.SwitchImage(resultScene.background))
                yield return new WaitForSeconds(1f);
            resultsSign.GetComponent<Animator>().SetTrigger("Show");
            yield return new WaitForSeconds(1f);

            #region TextBarAlignment
            // Move TextBar to the middle of the screen and Resize it
            RectTransform rectTransform = bottomBarController.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
            rectTransform.anchorMax = new Vector2(1.0f, 0.5f);
            rectTransform.offsetMin = new Vector2(260f, 0f);
            rectTransform.offsetMax = new Vector2(-260f, 0f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 500f);
            //rectTransform.anchoredPosition -= new Vector2(0f, 0f);

            // Realign and resize the text
            bottomBarController.barText.GetComponent<RectTransform>().offsetMin = new Vector2(30f, 20f);
            bottomBarController.barText.GetComponent<RectTransform>().offsetMax = new Vector2(-30f, -20f);
            bottomBarController.barText.fontSize = 100;
            #endregion

            bottomBarController.ClearText(resultScene);
            bottomBarController.Resize(resultScene);
            bottomBarController.Show();
            yield return new WaitForSeconds(1f);
            bottomBarController.PlayScene(resultScene as TextScene);
            state = State.IDLE;
        }

        else if (scene is DonateScene)
        {
            DonateScene donateScene = scene as DonateScene;
            PlayAudio(donateScene.sentences[0]);
            // Enable when we have 2 different background images for summary and donation screens
            /*backgroundController.SwitchImage(donateScene.background);
            yield return new WaitForSeconds(1f);*/

            #region TextBarAlignment
            // Move Text Background to the middle of the screen and Resize it
            RectTransform rectTransform = bottomBarController.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
            rectTransform.anchorMax = new Vector2(1.0f, 0.5f);
            rectTransform.offsetMin = new Vector2(260f, 0f);
            rectTransform.offsetMax = new Vector2(-260f, 0f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 260f);
            rectTransform.anchoredPosition += new Vector2(0f, 200f);
            // Realign and resize Text
            bottomBarController.barText.GetComponent<RectTransform>().offsetMin = new Vector2(30f, 10f);
            bottomBarController.barText.GetComponent<RectTransform>().offsetMax = new Vector2(-30f, -10f);
            bottomBarController.barText.fontSize = 100;
            #endregion

            bottomBarController.ClearText(donateScene);
            bottomBarController.Resize(donateScene);
            bottomBarController.Show();
            yield return new WaitForSeconds(1f);
            bottomBarController.PlayScene(donateScene as TextScene);
            state = State.IDLE;
        }

    }

    private void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound);
    }

    public void OpenDonationPage()
    {
        Application.OpenURL("https://www.snakebitefoundation.org/donate/");
    }

    public void OpenCreditsScene()
    {
        StartCoroutine(LoadLevel(creditsScene));
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
