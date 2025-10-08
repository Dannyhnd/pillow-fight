using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] uiElementsToHideAtStart;

    void Start()
    {
        foreach (GameObject uiElement in uiElementsToHideAtStart)
        {
            uiElement.SetActive(false);
        }
    }

    public void ShowUIElements()
    {
        foreach (GameObject uiElement in uiElementsToHideAtStart)
        {
            uiElement.SetActive(true);
        }
    }
}

