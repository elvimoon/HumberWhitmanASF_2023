using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeaker", menuName = "Data/New Speaker")]
[System.Serializable]

// EACH SPEAKER WILL BE HIGHLIGHTED IN A SPECIFIC COLOR

public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color textColor;

}
