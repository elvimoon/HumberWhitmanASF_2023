using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactEntry : MonoBehaviour
{
    public GameObject button;
    string buttonText_ORG;

    public bool locked = false;

    [SerializeField]
    private string title;

    [SerializeField]
    private string description;


    [SerializeField]
    private GameObject entryTitleText;
    [SerializeField]
    private GameObject entryText;


    // Start is called before the first frame update
    void Start()
    {
        entryTitleText = GameObject.FindGameObjectWithTag("EntryTitle");
        entryText = GameObject.FindGameObjectWithTag("EntryText");

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
    }

    public void ButtonClick()
    {
        entryTitleText.GetComponent<TextMeshProUGUI>().text = title;
        entryText.GetComponent<TextMeshProUGUI>().text = description;
    }
}
