using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damageInterval = 0.2f; // how often to deal damage (seconds)
    private Dictionary<CharacterMovement, float> damageTimers = new Dictionary<CharacterMovement, float>();

    void OnTriggerStay2D(Collider2D other)
    {
        CharacterMovement controller = other.GetComponent<CharacterMovement>();

        if (controller != null)
        {
            // Track time since last damage for this character
            if (!damageTimers.ContainsKey(controller))
                damageTimers[controller] = 0f;

            damageTimers[controller] += Time.deltaTime;

            if (damageTimers[controller] >= damageInterval)
            {
                controller.ChangeHealth(-10);
                damageTimers[controller] = 0f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CharacterMovement controller = other.GetComponent<CharacterMovement>();

        // Reset timer when player leaves zone
        if (controller != null && damageTimers.ContainsKey(controller))
        {
            damageTimers.Remove(controller);
        }
    }
} 