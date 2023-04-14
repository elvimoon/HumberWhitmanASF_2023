using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactEntry : MonoBehaviour
{
    public enum EntryCat { Category1, Category2, Category3, Category4 };
    public EntryCat entryCat;

    public GameObject button;
    string buttonText_ORG;

    public bool locked = false;
    [SerializeField]
    private bool hasImage = false;

    [SerializeField]
    private string title;
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite image;


    [SerializeField]
    private GameObject entryTitleText;
    [SerializeField]
    private GameObject entryText;
    [SerializeField]
    private GameObject entryImage;

    // Start is called before the first frame update
    void Start()
    {
        if (entryCat == EntryCat.Category1)
        {
            entryTitleText = GameObject.FindGameObjectWithTag("EntryTitle");
            entryText = GameObject.FindGameObjectWithTag("EntryText");
            entryImage = GameObject.FindGameObjectWithTag("EntryImage");
        }
        else if (entryCat == EntryCat.Category2)
        {
            entryTitleText = GameObject.FindGameObjectWithTag("EntryTitle2");
            entryText = GameObject.FindGameObjectWithTag("EntryText2");
            entryImage = GameObject.FindGameObjectWithTag("EntryImage2");
        }
        else if (entryCat == EntryCat.Category3)
        {
            entryTitleText = GameObject.FindGameObjectWithTag("EntryTitle3");
            entryText = GameObject.FindGameObjectWithTag("EntryText3");
            entryImage = GameObject.FindGameObjectWithTag("EntryImage3");
        }
        else if (entryCat == EntryCat.Category4)
        {
            entryTitleText = GameObject.FindGameObjectWithTag("EntryTitle4");
            entryText = GameObject.FindGameObjectWithTag("EntryText4");
            entryImage = GameObject.FindGameObjectWithTag("EntryImage4");
        }

        button = this.transform.GetChild(0).gameObject;
        buttonText_ORG = button.GetComponent<TextMeshProUGUI>().text.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
        {
            this.GetComponent<Button>().interactable = false;
            button.GetComponent<TextMeshProUGUI>().text = "Entry Locked";
        }
        else
        {
            this.GetComponent<Button>().interactable = true;
            button.GetComponent<TextMeshProUGUI>().text = buttonText_ORG;
        }

        //if (hasImage)
        //{
        //    entryText.GetComponent<TextMeshProUGUI>().rectTransform.position = new Vector3(470, -162.5f, 0.0f);
        //}
        //else
        //{

        //}
    }

    public void ButtonClick()
    {
        entryTitleText.GetComponent<TextMeshProUGUI>().text = title;
        entryText.GetComponent<TextMeshProUGUI>().text = description;
        entryImage.GetComponent<Image>().sprite = image;
    }
}
