using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockController : MonoBehaviour
{
    public GameObject codexMenu;

    // Start is called before the first frame update
    void Start()
    {
        codexMenu = GameObject.FindGameObjectWithTag("Codex Menu");
    }

    // Update is called once per frame
    void Update()
    {
        codexMenu.GetComponent<FactMenuController>().EntryUnlock(0, 0);

    }
}
