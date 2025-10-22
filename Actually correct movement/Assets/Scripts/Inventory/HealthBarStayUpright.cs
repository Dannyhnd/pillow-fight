using UnityEngine;

public class HealthBarPosition : MonoBehaviour
{
    [SerializeField] private Transform target; 
    [SerializeField] private float offsetY = 1f; 

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPos = target.position;
        newPos.y += offsetY;
        transform.position = newPos;

        transform.rotation = Quaternion.identity;
    }
}