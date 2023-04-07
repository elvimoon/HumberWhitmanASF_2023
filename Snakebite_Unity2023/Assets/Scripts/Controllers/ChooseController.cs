using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//this script will handle the logic of the selection screen itself
public class ChooseController : MonoBehaviour
{
    public ChooseLabelController label;
    public GameController gameController;
    private RectTransform rectTransform;
    private Animator animator;
    private float labelHeight = -1;
    private bool isClicked = false;

    //when selection screen is called, we want to generate labels
    void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupChoose(ChooseScene scene)
    {
        DestroyLabels();
        animator.SetTrigger("Show");

        for (int index = 0; index < scene.labels.Count; index++)
        {
            //instantiate allows us to create a new label inside our selection screen
            ChooseLabelController newLabel = Instantiate(label.gameObject, transform).GetComponent<ChooseLabelController>();

            //we need to get the height of the label, this will help in calculating their y-coordinates
            if (labelHeight == -1)
            {
                labelHeight = newLabel.GetHeight();
            }

            //pass coordinates, text and controller object to all labels
            newLabel.Setup(scene.labels[index], this, CalculateLabelPositions(index, scene.labels.Count));
        }

        Vector2 size = rectTransform.sizeDelta;
        size.y = (scene.labels.Count + 2) * labelHeight;
        rectTransform.sizeDelta = size;
    }

    //as soon as player clicks on label, hide selection screen and go to corresponding scene
    public void PerformChoose(StoryScene scene, SummaryScene summaryScene)
    {
        if (!isClicked)
        {
            if(summaryScene) gameController.endScenes.Enqueue(summaryScene);
            animator.SetTrigger("Hide");
            gameController.PlayScene(scene);
            isClicked = true;
        }
    }

    //we want that regardless of # of labels, they should always be in middle of the screen
    private float CalculateLabelPositions(int labelIndex, int labelCount)
    {
        //say we have label height of 50, for an even # of labels each will have y coordinates -75, -25, 25, 75...
        // odd # of labels will have y coordinates -50, 0, 50...
        if (labelCount %2 == 0)
        {
            if (labelIndex < labelCount /2)
            {
                return labelHeight * (labelCount / 2 - labelIndex - 1) + labelHeight / 2;
            }
            else
            {
                return -1 * (labelHeight * (labelIndex - labelCount /2) + labelHeight / 2);
            }
        }
        else
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex);
            }
            else if (labelIndex > labelCount /2)
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2));
            }
            else
            {
                return 0;
            }
        }
    }

    //to generate new labels when selection screen called, we need to first delete all existing labels
    private void DestroyLabels()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
        isClicked = false;
    }
}
