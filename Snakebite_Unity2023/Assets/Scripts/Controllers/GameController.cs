using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //changing scenes 

    public GameScene currentScene;
    public BottomBarController bottomBarController;
    public BackgroundController backgroundController;
    public ChooseController chooseController;
    public AudioController audioController;
    public GameObject resultsSign;
    public GameObject nextButton;
    
    private bool isNextButtonHidden = true;
    private State state = State.IDLE;

    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }

    void Start()
    {
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
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !(currentScene is SummaryScene))
        {
            if(state == State.IDLE && bottomBarController.IsCompleted())
            {
                if (bottomBarController.IsLastSentence())
                {
                    bottomBarController.SetWaitingState();
                    PlayScene((currentScene as StoryScene).nextScene);
                }

                else
                {
                    bottomBarController.PlayNextSentence();
                    PlayAudio((currentScene as StoryScene)
                        .sentences[bottomBarController.GetSentenceIndex()]);
                }
            }
        }else if(bottomBarController.IsCompleted() && currentScene is SummaryScene && isNextButtonHidden)
        {
            nextButton.GetComponent<Animator>().SetTrigger("Show");
            isNextButtonHidden = false;
        }
    }

    public void PlayScene(GameScene scene)
    {
        StartCoroutine(SwitchScene(scene));
    }

    //function to create a smooth transition between scenes using IEnumerator to wait for end of the anims
    private IEnumerator SwitchScene(GameScene scene)
    {
        //at beginning of anim, panel will disappear, then bg will change, then panel will appear
        state = State.ANIMATE;
        if(currentScene is SummaryScene) resultsSign.GetComponent<Animator>().SetTrigger("Hide");
        yield return new WaitForSeconds(1f);
        currentScene = scene;
        bottomBarController.Hide();
        yield return new WaitForSeconds(1f);

        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            backgroundController.SwitchImage(storyScene.background);
            PlayAudio(storyScene.sentences[0]);
            yield return new WaitForSeconds(1f);
            bottomBarController.ClearText(storyScene);
            bottomBarController.Show();
            yield return new WaitForSeconds(1f);
            bottomBarController.PlayScene(storyScene);
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
            backgroundController.SwitchImage(summaryScene.background);
            PlayAudio(summaryScene.sentences[0]);
            yield return new WaitForSeconds(1f);

            resultsSign.GetComponent<Animator>().SetTrigger("Show");

            yield return new WaitForSeconds(1f);

            // Move TextBar to the middle of the screen
            RectTransform rectTransform = bottomBarController.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
            rectTransform.anchorMax = new Vector2(1.0f, 0.5f);
            rectTransform.anchoredPosition -= new Vector2(0f, 100f);
            rectTransform.offsetMin = new Vector2(100f, 0f);
            rectTransform.offsetMax = new Vector2(-100f, 0f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 300f);

            // Realign and resize the text
            bottomBarController.barText.GetComponent<RectTransform>().offsetMin = new Vector2(30f, 30f);
            bottomBarController.barText.GetComponent<RectTransform>().offsetMax = new Vector2(-30f, -30f);
            bottomBarController.barText.fontSize = 72;

            bottomBarController.ClearText(summaryScene);
            bottomBarController.Show();
            yield return new WaitForSeconds(1f);
            bottomBarController.PlayScene(summaryScene as TextScene);
            state = State.IDLE;
        }

    }

    private void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound);
    }

}
