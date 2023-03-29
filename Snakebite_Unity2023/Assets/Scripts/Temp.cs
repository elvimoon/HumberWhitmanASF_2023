using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetComponent<CanvasGroup>().alpha);
    }
}
