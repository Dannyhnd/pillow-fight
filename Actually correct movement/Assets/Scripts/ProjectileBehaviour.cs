using UnityEngine;
using Unity.Netcode;

public class ProjectileBehaviour : NetworkBehaviour
{
    public float Speed = 5f;
    public int damage = 20;

    private void Update()
    {
        // Only the server moves the projectile
        if (!IsServer) return;

        transform.position += transform.right * Time.deltaTime * Speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return; // Only the server handles collisions & damage

        // Ignore players
        if (other.CompareTag("Player")) return;

        // Damage enemy if present
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Despawn projectile safely
        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn();
        }
    }
}
