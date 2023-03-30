using System.Collections.Generic;
using UnityEngine;
using static StoryScene;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]

// THIS SCENE WILL CONTAIN A BACKGROUND, A LIST OF SENTENCES TO BE PLAYED, AND THE NEXT SCENE
public class StoryScene : TextScene
{
    public Sprite background;
    public GameScene nextScene;    
}

public class TextScene : GameScene
{
    public List<Sentence> sentences;

    // For sentences, we create a separate structure, which also includes the speaker
    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;

        public AudioClip music;
        public AudioClip sound;
    }
}

//combine choosescene and storyscene into a new class called gamescene
public class GameScene : ScriptableObject
{

}