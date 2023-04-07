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
    public int time = 0;
    public bool isTimeVisible = false;
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
        public List<Action> actions;
        public AudioClip music;
        public AudioClip sound;

        [System.Serializable]

        public struct Action
        {
            public Speaker speaker;
            public int spriteIndex;
            public Type actionType;
            public Vector2 coords;
            public float moveSpeed;

            [System.Serializable]

            public enum Type
            {
                NONE, APPEAR, MOVE, DISAPPEAR
            }
        }
    }
}

//combine choosescene and storyscene into a new class called gamescene
public class GameScene : ScriptableObject
{

}