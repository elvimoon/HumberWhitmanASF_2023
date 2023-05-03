using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryController : MonoBehaviour
{
    public enum EntryCat { Category1, Category2, Category3, Category4 };
    public EntryCat entryCat;

    public GameObject[] entries;

    [SerializeField]
    private int currentEntry;

    public GameObject codexMenu;
    // Start is called before the first frame update
    void Start()
    {

        codexMenu = GameObject.FindGameObjectWithTag("Codex Menu");

        if (entryCat == EntryCat.Category1)
        {
            entries = GameObject.FindGameObjectsWithTag("Entry1");
        }
        else if (entryCat == EntryCat.Category2)
        {
            entries = GameObject.FindGameObjectsWithTag("Entry2");
        }
        else if(entryCat == EntryCat.Category3)
        {
            entries = GameObject.FindGameObjectsWithTag("Entry3");
        }
        else if(entryCat == EntryCat.Category4)
        {
            entries = GameObject.FindGameObjectsWithTag("Entry4");
        }

        currentEntry = 0;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void UnlockEntry(int index)
    {
        entries[index].GetComponent<FactEntry>().locked = false;
        codexMenu.GetComponent<FactMenuController>().newEntryText.text = "!";

    }
}
