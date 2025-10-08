using UnityEngine;

public class ShowUIOnClick : MonoBehaviour
{
    public GameObject[] uiElementsToShow;

    public void ShowElements()
    {
        foreach (GameObject element in uiElementsToShow)
        {
            element.SetActive(true);
        }
    }
}

