using UnityEngine;
using Unity.Netcode;

public class Enemy : NetworkBehaviour
{
    public int maxHealth = 100;

    // NetworkVariable syncs health automatically across clients
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    public FloatingHealthBar healthBar;

    void Start()
    {
        // Only set max health once on the server
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }

        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);

        // Listen for health changes
        currentHealth.OnValueChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        currentHealth.OnValueChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(newValue);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return; // Only server modifies health

        currentHealth.Value -= damage;

        if (currentHealth.Value <= 0)
        {
            NetworkObject.Despawn(); // Proper networked destruction
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return; // Only server handles damage

        if (other.CompareTag("Projectile"))
        {
            ProjectileBehaviour projectile = other.GetComponent<ProjectileBehaviour>();
            if (projectile != null)
            {
                TakeDamage(projectile.damage);

                // Despawn projectile on the server
                if (projectile.NetworkObject != null && projectile.NetworkObject.IsSpawned)
                {
                    projectile.NetworkObject.Despawn();
                }
            }
        }
    }
}
