using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryController : MonoBehaviour
{

    public GameObject[] entries;

    [SerializeField]
    private int currentEntry;

    public GameObject codexMenu;
    // Start is called before the first frame update
    void Start()
    {

        codexMenu = GameObject.FindGameObjectWithTag("Codex Menu");
        entries = GameObject.FindGameObjectsWithTag("Entry");

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
