using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonHoverTextChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    ////////////////////////////////////////////////////////////////////////////////////////////
    // This script synchronizes text color with button color, when it is seleceted or pressed //
    ////////////////////////////////////////////////////////////////////////////////////////////

    private TextMeshProUGUI txt;
    private Button btn;
    private Color baseColor;
 
    void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
        btn = gameObject.GetComponent<Button>();
        baseColor = txt.color;
    }

    private bool isPressed = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        txt.color = baseColor * btn.colors.highlightedColor * btn.colors.colorMultiplier;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        txt.color = baseColor * btn.colors.pressedColor * btn.colors.colorMultiplier;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        txt.color = baseColor * btn.colors.normalColor * btn.colors.colorMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if(!isPressed)
        {
            txt.color = baseColor * btn.colors.normalColor * btn.colors.colorMultiplier;
        }
    }
}