using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public bool isSwitched = false;
    public Image background1;
    public Image background2;
    public Animator animator;

    //switch first = first -> second
    //function to smoothly change background and a function to force it to change
    public bool SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            if (sprite == background1.sprite) return false;
            background2.sprite = sprite;
            animator.SetTrigger("SwitchFirst");
        }
        else
        {
            if (sprite == background2.sprite) return false;
            background1.sprite = sprite;
            animator.SetTrigger("SwitchSecond");
        }
        isSwitched = !isSwitched;
        return true;
    }

    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            background1.sprite = sprite;
        }
        else
        {
            background2.sprite = sprite;
        }
    }
}
