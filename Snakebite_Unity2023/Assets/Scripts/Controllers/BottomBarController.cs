using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BottomBarController : MonoBehaviour
{
    
    // display text by character, to do this we need to know what scene we are on now and what sentence needs to be displayed
    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;

    private int sentenceIndex = -1;
    private TextScene currentScene;
    private State state = State.COMPLETED;
    private Animator animator;
    private bool isHidden = true;

    public Dictionary<Speaker, SpriteController> sprites;
    public GameObject spritesPrefab;

    //will also create an enum state in order to understand whether we have displayed everything or not
    private enum State
    {
        WAITING, PLAYING, COMPLETED
    }

    private void Awake()
    {
        sprites = new Dictionary<Speaker, SpriteController>();
        animator = GetComponent<Animator>();

    }

    public int GetSentenceIndex()
    {
        return sentenceIndex;
    }

    public void SetWaitingState() { state = State.WAITING; }

    //function to hide the bottombar via anim
    public void Hide()
    {
        if (!isHidden)
        {
            animator.SetTrigger("Hide");
            isHidden = true;
        } 
    }

    //function to show the bottombar via anim
    public void Show()
    {
        if (isHidden)
        {
            animator.SetTrigger("Show");
            isHidden = false;
        }
    }

    //function to clear the text, which will be used before appearance of the panel
    public void ClearText(TextScene scene)
    {
        barText.text = "";
        //set initial speaker and color
        personNameText.text = scene.sentences[0].speaker.speakerName;
        personNameText.color = scene.sentences[0].speaker.textColor;
    }

    public void Resize(TextScene scene)
    {
        string temp = barText.text;
        barText.text = scene.sentences[0].text;
        GetComponent<RectTransform>().sizeDelta =
                new Vector2(GetComponent<RectTransform>().sizeDelta.x,
                barText.GetPreferredValues().y + 30);
        Debug.Log(barText.GetPreferredValues().y);
        barText.text = temp;
    }

    public void ResizeText(TextScene scene)
    {
        barText.autoSizeTextContainer = true;
        string temp = barText.text;
        barText.text = scene.sentences[0].text;
        float size = barText.fontSize;
        barText.autoSizeTextContainer = false;
        barText.fontSize = size;
        Debug.Log(size);
        barText.text = temp;
    }

    public void PrintEverything(TextScene scene)
    {
        StopAllCoroutines();
        barText.text = scene.sentences[sentenceIndex].text;
        state = State.COMPLETED;
    }

    public void PlayScene(TextScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
        PlayNextSentence();
    }

    public void PlayNextSentence()
    {
        //set text to display sequentially as though it's typed out, see the IEnumerator TypeText
        StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
        //set speaker and color
        personNameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
        personNameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
        ActSpeakers();
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count;
    }

    //in order to not block main thread, we display the text in a new one (for this we mark function as ienum)
    private IEnumerator TypeText(string text)
    {
        barText.text = "";
        state = State.PLAYING;
 
        //this will allow us to pause between the output of letters, which will create a typing effect
        int wordIndex = 0;
        while (state != State.COMPLETED)
        {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(0.02f);

            if (++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }
    }
    private void ActSpeakers()
    {
        List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceIndex].actions;
        for(int i = 0; i < actions.Count; i++)
        {
            ActSpeaker(actions[i]);
        }
    }

    private void ActSpeaker(StoryScene.Sentence.Action action)
    {
        SpriteController controller = null;
        switch (action.actionType)
        {
            case StoryScene.Sentence.Action.Type.APPEAR:
                if (!sprites.ContainsKey(action.speaker))
                {
                    controller = Instantiate(action.speaker.prefab.gameObject, spritesPrefab.transform)
                        .GetComponent<SpriteController>();
                    sprites.Add(action.speaker, controller);
                }
                else
                {
                    controller = sprites[action.speaker];
                }
                controller.Setup(action.speaker.sprites[action.spriteIndex]);
                controller.Show(action.coords);
                return;
            case StoryScene.Sentence.Action.Type.MOVE:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                    controller.Move(action.coords, action.moveSpeed);
                }
                break;
            case StoryScene.Sentence.Action.Type.DISAPPEAR:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                    controller.Hide();
                }
                break;
            case StoryScene.Sentence.Action.Type.NONE:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                }
                break;
        }
        if (controller != null)
        {
            controller.SwitchSprite(action.speaker.sprites[action.spriteIndex]);
        }
    }

}
