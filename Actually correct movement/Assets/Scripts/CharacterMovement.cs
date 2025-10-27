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
using Unity.Netcode;

public class CharacterMovement : NetworkBehaviour
{
    public float speed = 5f;
    public ProjectileBehaviour projectilePrefab;
    public Transform launchOffset;
    public Slider healthBar;

    // Health
    public int maxHealth = 5;
    //private int currentHealth;

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server); 

    private Vector3 spawnPosition;

    void Start()
    {
        currentHealth.Value = maxHealth;
        spawnPosition = transform.position;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth.Value;
        } else
        {
            healthBar = GameObject.Find("Healthbar").GetComponent<Slider>();
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
        if (!IsServer) return; // Only server changes health

        currentHealth.Value = Mathf.Clamp(currentHealth.Value + amount, 0, maxHealth);

        if (currentHealth.Value <= 0)
        {
            Respawn();
        }
    }

    /**
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            // Get damage from projectile
            ProjectileBehaviour projectile = other.GetComponent<ProjectileBehaviour>();
            if (projectile != null)
            {
                ChangeHealth(-1); // Apply damage
            }

            Destroy(other.gameObject); // Remove projectile
        }
    }
    */

    private void Respawn()
    {
        transform.position = spawnPosition;
        currentHealth.Value = maxHealth;

        if (healthBar != null)
        {
            healthBar.value = currentHealth.Value;
        }
    }
    
    public override void OnNetworkSpawn()
    {
        currentHealth.OnValueChanged += (oldValue, newValue) =>
        {
            Debug.Log($"Health changed from {oldValue} to {newValue}");
            if (healthBar != null)
            {
                healthBar.value = newValue;
            }
        };

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth.Value;
        }
    }


}


