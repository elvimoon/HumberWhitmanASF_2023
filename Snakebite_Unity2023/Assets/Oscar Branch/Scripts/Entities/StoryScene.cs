using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]

// THIS SCENE WILL CONTAIN A BACKGROUND, A LIST OF SENTENCES TO BE PLAYED, AND THE NEXT SCENE
public class StoryScene : GameScene
{
    public List<Sentence> sentences;
    public Sprite background;
    public GameScene nextScene;

    // For sentences, we create a separate structure, which also includes the speaker
    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;
    }
}

//combine choosescene and storyscene into a new class called gamescene
public class GameScene : ScriptableObject
{

}