using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ClickableText : MonoBehaviour, IPointerClickHandler
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    // This script enables functionality to click on links that are embedded into text boxes //
    ///////////////////////////////////////////////////////////////////////////////////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        var text = GetComponent<TextMeshProUGUI>();
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
            if(linkIndex > -1)
            {
                var linkInfo = text.textInfo.linkInfo[linkIndex];
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    }
}
