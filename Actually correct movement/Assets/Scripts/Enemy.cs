using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public FloatingHealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthBar != null) healthBar.SetHealth(currentHealth);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect projectile hit
        if (other.CompareTag("Projectile"))
        {
            ProjectileBehaviour projectile = other.GetComponent<ProjectileBehaviour>();
            if (projectile != null)
            {
                TakeDamage(projectile.damage);
                Destroy(other.gameObject);
            }
        }
    }
}
