using UnityEngine;

public class HideUIOnClick : MonoBehaviour
{
    public GameObject uiElementToHide;
    public GameObject buttonToHide;

    public void HideElements()
    {
        Destroy(uiElementToHide);
        Destroy(buttonToHide);
    }
    
}

