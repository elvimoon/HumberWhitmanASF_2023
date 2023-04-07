using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FactMenuController : MonoBehaviour
{
    public TextMeshProUGUI newEntryText;

    public bool menuOn;
    public bool debug = false;

    GameObject menuBody;
    [SerializeField]
    private GameObject[] menuCategories;
    [SerializeField]
    private GameObject[] buttonCategories;
    public ColorBlock selectedColor;
    [SerializeField]
    private int currentCategory;

    [SerializeField]
    private string[] menuCategoriesText;
    [SerializeField]
    private string[] menuCategoriesTitle;

    [SerializeField]
    private GameObject entryTitleText;
    [SerializeField]
    private GameObject entryText;


    // Start is called before the first frame update
    void Start()
    {
        entryTitleText = GameObject.FindGameObjectWithTag("EntryTitle");
        entryText = GameObject.FindGameObjectWithTag("EntryText");

        menuBody = this.transform.GetChild(0).gameObject;
        menuOn = false;
        menuCategories = GameObject.FindGameObjectsWithTag("Menu Category");
        buttonCategories = GameObject.FindGameObjectsWithTag("Button Category");

        foreach (GameObject button in buttonCategories)
        {
            button.GetComponent<Button>().colors = selectedColor;
        }

        currentCategory = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            ShowCodex();
        }

        if (menuOn)
        {
            menuBody.SetActive(true);
        }
        else
        {
            menuBody.SetActive(false);
        }

        if (Input.GetKeyDown("w") && (debug))
        {
            menuCategories[0].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(Random.Range(0, menuCategories[0].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length));
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Entry Unlocked");
        }

    }

    void ShowCodex()
    {
        if (menuOn)
        {
            if (debug)
            {
                print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Fact menu closed");
            }
            menuOn = false;
        }
        else
        {
            if (debug)
            {
                print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Fact menu opened");
            }
            menuOn = true;
            ShowCategory(currentCategory);
            buttonCategories[currentCategory].GetComponent<Button>().Select();
            newEntryText.text = "";
        }
    }

    public void CodexButtonClick()
    {
        ShowCodex();
    }

    public void ShowCategory(int index)
    {
        foreach (GameObject category in menuCategories)
        {
            category.SetActive(false);
        }

        menuCategories[index].SetActive(true);
        currentCategory = index;

        entryTitleText.GetComponent<TextMeshProUGUI>().text = menuCategoriesTitle[index];
        entryText.GetComponent<TextMeshProUGUI>().text = menuCategoriesText[index];

        if (debug)
        {
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Category(" + index + ") active");
        }
    }

}
