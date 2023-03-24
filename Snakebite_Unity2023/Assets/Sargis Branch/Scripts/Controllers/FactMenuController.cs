using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FactMenuController : MonoBehaviour
{
    public bool menuOn;
    public bool debug = false;

    GameObject menuBody;
    public GameObject[] menuCategories;
    public GameObject[] buttonCategories;
    public ColorBlock selectedColor;
    int currentCategory;

    // Start is called before the first frame update
    void Start()
    {
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

        if (debug)
        {
            print("Debug - " + this.gameObject.GetComponent<MonoBehaviour>() + ": Category(" + index + ") active");
        }
    }

}
