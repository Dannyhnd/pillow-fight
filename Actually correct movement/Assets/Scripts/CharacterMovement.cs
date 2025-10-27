/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterMovement : NetworkBehaviour //changed for multiplayer
{
    public float speed = 10.4f;

    public ProjectileBehaviour ProjectilePrefab;
    public Transform LaunchOffset;
    //Make player look red (to differentiate using multiplayer)
    void Start()
    {
        if (IsOwner)
        {
            //GetComponent<Renderer>().meterial.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return; //check if player is controlling this character

        Vector3 pos = transform.position;

        if (Input.GetKey("w")) 
        {
            pos.y += speed * Time.deltaTime;
        }

         if (Input.GetKey("s")) 
        {
            pos.y -= speed * Time.deltaTime;
        }

         if (Input.GetKey("d")) 
        {
            pos.x += speed * Time.deltaTime;
        }

         if (Input.GetKey("a")) 
        {
            pos.x -= speed * Time.deltaTime;
        }

        transform.position = pos;
    }
}
*/
//Script for player to take damage
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5f;
    public ProjectileBehaviour projectilePrefab;
    public Transform launchOffset;
    public Slider healthBar;

    // Health
    public int maxHealth = 5;
    private int currentHealth;

    private Vector3 spawnPosition;

    void Start()
    {
        currentHealth = maxHealth;
        spawnPosition = transform.position;

        currentHealth = maxHealth;
        spawnPosition = transform.position;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w")) pos.y += speed * Time.deltaTime;
        if (Input.GetKey("s")) pos.y -= speed * Time.deltaTime;
        if (Input.GetKey("d")) pos.x += speed * Time.deltaTime;
        if (Input.GetKey("a")) pos.x -= speed * Time.deltaTime;

        transform.position = pos;
    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(projectilePrefab, launchOffset.position, Quaternion.identity);
        }
    }


    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Respawn();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            ChangeHealth(-1);
            Destroy(other.gameObject);
        }
    }

     private void Respawn()

    {
        transform.position = spawnPosition;
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

}


