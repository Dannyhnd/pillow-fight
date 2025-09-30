using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Button_pressed : MonoBehaviour
{
    public Button button;
    private bool isheld;
    private Image buttonImage;

    void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(ToggleHeld);
            buttonImage = button.GetComponent<Image>();
        }
    }

    void ToggleHeld()
    {
        isheld = !isheld;
        Debug.Log(isheld ? "Button is held down" : "Button released");
    }

    void Update()
    {
        if (isheld == true)
        {
            buttonImage  = Color.orange;
        }
        else
        {
            buttonImage = Color.white;
        }
}
