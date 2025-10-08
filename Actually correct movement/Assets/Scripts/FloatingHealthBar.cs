using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Slider slider;

    void Awake()
    {
        if (slider == null) slider = GetComponentInChildren<Slider>();
    }

    public void SetMaxHealth(int health)
    {
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
        }
    }

    public void SetHealth(int health)
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }
}
