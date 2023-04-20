using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public bool isSwitched = false;
    public Image image1;
    public Image image2;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //switch first = first -> second
    //function to smoothly change background and a function to force it to change
    public bool SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            if (sprite == image1.sprite) return false;
            image2.sprite = sprite;
            animator.SetTrigger("SwitchFirst");
        }
        else
        {
            if (sprite == image2.sprite) return false;
            image1.sprite = sprite;
            animator.SetTrigger("SwitchSecond");
        }
        isSwitched = !isSwitched;
        return true;
    }

    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            image1.sprite = sprite;
        }
        else
        {
            image2.sprite = sprite;
        }
    }

    public Sprite GetImage()
    {
        if (!isSwitched)
        {
            return image1.sprite;
        }
        else
        {
            return image2.sprite;
        }
    }
}
