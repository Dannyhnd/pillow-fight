/*using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed = 4.5f;
    private void Update()
    {
        transform.position += -transform.right * Time.deltaTime * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        Destroy(gameObject);
    }
}
*/
//Projectile behaviour after adding health bar


using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed = 5f;
    public int damage = 20;

    private Rigidbody2D rb;
    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore player
        if (other.CompareTag("Player")) return;

        // Damage enemy
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destroy projectile on hit
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        Destroy(gameObject);
    }
}
