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
    public bool cheats = false;

    GameObject menuBody;
    public GameObject[] menuCategories;
    [SerializeField]
    private GameObject[] buttonCategories;
    public ColorBlock selectedColor;
    [SerializeField]
    public int currentCategory;

    [SerializeField]
    private string[] menuCategoriesText;
    [SerializeField]
    private string[] menuCategoriesTitle;
    [SerializeField]
    private Sprite[] menuCategoriesImage;

    [SerializeField]
    private GameObject entryTitleText;
    [SerializeField]
    private GameObject entryText;
    public GameObject entryImage;

    // Start is called before the first frame update
    void Start()
    {
        entryTitleText = GameObject.FindGameObjectWithTag("EntryTitle");
        entryText = GameObject.FindGameObjectWithTag("EntryText");
        entryImage = GameObject.FindGameObjectWithTag("EntryImage");

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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(2))
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

        if (Input.GetKeyDown("z") && (debug) && (cheats))
        {
            menuCategories[0].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(Random.Range(0, menuCategories[0].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length));
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Entry From Catgory 1 Unlocked");
        }
        if (Input.GetKeyDown("x") && (debug) && (cheats))
        {
            menuCategories[1].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(Random.Range(0, menuCategories[1].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length));
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Entry From Catgory 2 Unlocked");
        }
        if (Input.GetKeyDown("c") && (debug) && (cheats))
        {
            menuCategories[2].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(Random.Range(0, menuCategories[2].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length));
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Entry From Catgory 3 Unlocked");
        }
        if (Input.GetKeyDown("v") && (debug) && (cheats))
        {
            menuCategories[3].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(Random.Range(0, menuCategories[3].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length));
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Entry From Catgory 4 Unlocked");
        }

        if (Input.GetKey(KeyCode.UpArrow) && (cheats))
        {
            if ((Input.GetKey("a") && Input.GetKey("s") && Input.GetKey("f")) && (cheats))
            {
                for (int i = 0; i < menuCategories[0].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length; i++)
                {
                    menuCategories[0].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(i);
                }
                for (int i = 0; i < menuCategories[1].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length; i++)
                {
                    menuCategories[1].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(i);
                }
                for (int i = 0; i < menuCategories[2].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length; i++)
                {
                    menuCategories[2].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(i);
                }
                for (int i = 0; i < menuCategories[3].transform.GetChild(0).gameObject.GetComponent<EntryController>().entries.Length; i++)
                {
                    menuCategories[3].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(i);
                }

                if (debug)
                {
                    print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": All Entries Have Been Unlocked");
                }
            }
        }


        //Closes the menu if players clicks outside of it
        if (Input.GetMouseButton(0) && menuOn && !this.GetComponent<RectTransform>().rect.Contains(this.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition)))
        {
            ShowCodex();
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
        entryImage.GetComponent<Image>().sprite = menuCategoriesImage[index];

        if (debug)
        {
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Category(" + index + ") active");
        }
    }

    public void EntryUnlock(int catagory, int entry)
    {
        menuCategories[catagory].transform.GetChild(0).gameObject.GetComponent<EntryController>().UnlockEntry(entry);
        if (debug)
        {
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Entry(" + entry + ") From Category(" + catagory + ") Unlocked");
        }
    }
}
