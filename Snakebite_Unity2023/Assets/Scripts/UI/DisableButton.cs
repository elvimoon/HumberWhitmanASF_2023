using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisableButton : MonoBehaviour
{
    private const float DELAY = 1.0f;
    private Button button;

    //[SerializeField] Button otherButton;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        button.interactable = false;
        //otherButton.interactable = !otherButton.interactable;
        StartCoroutine(EnableButtonAfterDelay());
    }

    private IEnumerator EnableButtonAfterDelay()
    {
        yield return new WaitForSeconds(DELAY);
        button.interactable = true;
    }
}