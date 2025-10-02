using TMPro;
using UnityEngine;

public class collision : MonoBehaviour
{
    private int coinCounter = 0;
    public TMP_Text counterText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coin") && collision.gameObject.activeSelf == true)
        {
            collision.gameObject.SetActive(false);
            coinCounter += 1;
            counterText.text = "Coins: " + coinCounter;
        }
        
    }

}
