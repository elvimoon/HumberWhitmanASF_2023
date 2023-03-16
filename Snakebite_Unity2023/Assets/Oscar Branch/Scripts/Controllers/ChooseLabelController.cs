using UnityEngine;
using TMPro;
//to use IPointerHandler events, need to include UnityEngine.EventSystems
using UnityEngine.EventSystems;


//hovering over label will change done, done via implemention of IPointerHandler events
public class ChooseLabelController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColor;
    public Color hoverColor;
    private StoryScene scene;
    private TextMeshProUGUI textMesh;
    private ChooseController controller;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.color = defaultColor;
    }

    public float GetHeight()
    {
        return textMesh.rectTransform.sizeDelta.y * textMesh.rectTransform.localScale.y;
    }

    //need a controller object to handle all the clicks on the label
    public void Setup(ChooseScene.ChooseLabel label, ChooseController controller, float y)
    {
        scene = label.nextScene;
        textMesh.text = label.text;
        this.controller = controller;

        Vector3 position = textMesh.rectTransform.localPosition;
        position.y = y;
        textMesh.rectTransform.localPosition = position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.PerformChoose(scene);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //change color when hovered, change back when cursor leaves label area
        textMesh.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //change color when hovered, change back when cursor leaves label area
        textMesh.color = defaultColor;
    }
}

