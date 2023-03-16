using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]

// THIS SCENE WILL CONTAIN A BACKGROUND, A LIST OF SENTENCES TO BE PLAYED, AND THE NEXT SCENE
public class StoryScene : ScriptableObject
{
    public List<Sentence> sentences;
    public Sprite background;
    public StoryScene nextScene;

    // For sentences, we create a separate structure, which also includes the speaker
    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;
    }
}
