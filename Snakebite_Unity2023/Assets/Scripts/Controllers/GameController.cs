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
    public static GameController Instance;

    [SerializeField] private GameScene currentScene;
    [SerializeField] private BottomBarController bottomBarController;
    [SerializeField] private BackgroundController backgroundController;
    [SerializeField] private ChooseController chooseController;
    [SerializeField] private AudioController audioController;
    
    [SerializeField] private GameObject resultsSign;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject donateButton;
    [SerializeField] private GameObject endButton;
    [SerializeField] private GameObject timeBar;
    [SerializeField] private TextMeshProUGUI timeText;
    
    [SerializeField] private Animator levelTransition;
    [SerializeField] private string creditsScene;
    private const float TRANSITION_TIME = 1f;
    
    public Queue<TextScene> endScenes = new Queue<TextScene>();
    
    private int totalTime = 0;
    private GameScene dScene;
    private GameScene badEndingScene;
    private GameScene neutralEndingScene;
    private GameScene goodEndingScene;

    public bool isMenuOpen = false;
    public bool isSceneLoaded = false;
    private bool isFirstEndSceen = true;
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
        // endScenes.Enqueue(Resources.Load("Story/Scenes/Ending/Summary1A") as TextScene);
        // endScenes.Enqueue(Resources.Load("Story/Scenes/Ending/Summary1B") as TextScene);
        // endScenes.Enqueue(Resources.Load("Story/Scenes/Ending/Summary2A") as TextScene);
        // endScenes.Enqueue(Resources.Load("Story/Scenes/Ending/Summary2B") as TextScene);
        dScene = Resources.Load("Story/Scenes/Ending/Donate") as DonateScene;
        badEndingScene = Resources.Load("Story/Scenes/Ending/BadEnding") as ResultScene;
        neutralEndingScene = Resources.Load("Story/Scenes/Ending/NeutralEnding") as ResultScene;
        goodEndingScene = Resources.Load("Story/Scenes/Ending/GoodEnding") as ResultScene;
        StartCoroutine(OnStart());
    }

    private IEnumerator OnStart()
    {
        StoryScene storyScene = currentScene as StoryScene;
        backgroundController.SetImage(storyScene.background);
        yield return new WaitForSeconds(1.5f);
        bottomBarController.ClearText(storyScene);
        bottomBarController.Show();
        PlayAudio(storyScene.sentences[0]);
        yield return new WaitForSeconds(1f);
        bottomBarController.PlayScene(storyScene);
        isSceneLoaded = true;
    }

    //pressing spacebar or left mouse button will display the following sentence
    void Update()
    {
        // Set auto font size so that it auto adjust if resolution is changed
        if (bottomBarController.IsCompleted() && currentScene is SummaryScene && !bottomBarController.barText.enableAutoSizing)
        {
            bottomBarController.SetAutoFontSize(true);
        }
        // Show button only after Text is completed
        else if (bottomBarController.IsCompleted() && currentScene is ResultScene && isNextButtonHidden)
        {
            bottomBarController.SetAutoFontSize(true);
            nextButton.GetComponent<Animator>().SetTrigger("Show");
            isNextButtonHidden = false;
        }
        // Show buttons only after Text is completed
        else if (bottomBarController.IsCompleted() && currentScene is DonateScene && isDonateButtonHidden)
        {
            bottomBarController.SetAutoFontSize(true);
            StartCoroutine(ShowDonateSceneButtons());
            isDonateButtonHidden = false;
        }
    }

    public void ProcessInput()
    {
        if (currentScene is DonateScene || isMenuOpen || !isSceneLoaded) return;
        if(state == State.IDLE && bottomBarController.IsCompleted() && currentScene is not ResultScene)
        {
            if (bottomBarController.IsLastSentence())
            {
                if (currentScene is SummaryScene || currentScene is ResultScene)
                    PlayEndScenes();
                else if ((currentScene as StoryScene)?.nextScene)
                    PlayScene((currentScene as StoryScene)?.nextScene);
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
        else if(state == State.IDLE && !bottomBarController.IsCompleted()) {
            bottomBarController.FastForwardText(currentScene as TextScene);
        }
    }
    private IEnumerator ShowDonateSceneButtons()
    {
        yield return new WaitForSeconds(3f);
        donateButton.GetComponent<Animator>().SetTrigger("Show");
        yield return new WaitForSeconds(8f);
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
            timeBar.GetComponent<Animator>().SetTrigger("Hide");
            switch (totalTime)
            {
                case 0 :
                    endScenes.Enqueue(badEndingScene as TextScene);
                    break;
                case 17:
                    endScenes.Enqueue(neutralEndingScene as TextScene);
                    break;
                case 14:
                    endScenes.Enqueue(goodEndingScene as TextScene);
                    break;
            }
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
        if(currentScene is ResultScene) nextButton.GetComponent<Animator>().SetTrigger("Hide");
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
            yield return new WaitForSeconds(2f);
            bottomBarController.PlayScene(storyScene);

            totalTime += storyScene.time;
            timeText.text = "Hours Since Bitten: " + totalTime.ToString();
            if (storyScene.isTimeVisible)
            {
                timeBar.GetComponent<Animator>().SetTrigger("Show");
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
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 800f);
            //rectTransform.anchoredPosition -= new Vector2(0f, 50f);

            // Realign and resize the text
            bottomBarController.barText.GetComponent<RectTransform>().offsetMin = new Vector2(30f, 20f);
            bottomBarController.barText.GetComponent<RectTransform>().offsetMax = new Vector2(-30f, -20f);
            bottomBarController.barText.fontSize = 100;
            #endregion

            bottomBarController.ClearText(summaryScene);
            bottomBarController.ResizeText(summaryScene);
            bottomBarController.Show();
            yield return new WaitForSeconds(2f);
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
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 600f);
            rectTransform.anchoredPosition -= new Vector2(0f, 50f);

            // Realign and resize the text
            bottomBarController.barText.GetComponent<RectTransform>().offsetMin = new Vector2(30f, 20f);
            bottomBarController.barText.GetComponent<RectTransform>().offsetMax = new Vector2(-30f, -20f);
            bottomBarController.barText.fontSize = 100;
            #endregion

            bottomBarController.ClearText(resultScene);
            bottomBarController.ResizeText(resultScene);
            bottomBarController.Show();
            yield return new WaitForSeconds(1f);
            bottomBarController.PlayScene(resultScene as TextScene);
            state = State.IDLE;
        }

        else if (scene is DonateScene)
        {
            DonateScene donateScene = scene as DonateScene;
            PlayAudio(donateScene.sentences[0]);
            if (backgroundController.SwitchImage(donateScene.background))
                yield return new WaitForSeconds(1f);

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
            bottomBarController.barText.alignment = TextAlignmentOptions.Center;
            bottomBarController.barText.GetComponent<RectTransform>().offsetMin = new Vector2(30f, 10f);
            bottomBarController.barText.GetComponent<RectTransform>().offsetMax = new Vector2(-30f, -10f);
            bottomBarController.barText.fontSize = 100;
            #endregion
            
            bottomBarController.PrintAll(donateScene);
            bottomBarController.Show(0.33f);
            yield return new WaitForSeconds(3.0f);
            state = State.IDLE;
        }

    }

    public void SwitchIsMenuOpen()
    {
        isMenuOpen = !isMenuOpen;
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
        levelTransition.SetTrigger("Start");

        yield return new WaitForSeconds(TRANSITION_TIME);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
