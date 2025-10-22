using UnityEngine;

public class HealthBarPosition : MonoBehaviour
{
    [SerializeField] private Transform target; // The object this health bar belongs to
    [SerializeField] private float offsetY = 1f; // Adjust as needed

    void LateUpdate()
    {
        if (target == null) return;

        // Get object's position and add offset
        Vector3 newPos = target.position;
        newPos.y += offsetY;
        transform.position = newPos;

        // Keep upright
        transform.rotation = Quaternion.identity;
    }
}