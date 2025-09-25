using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 10.4f;

    public ProjectileBehaviour ProjectilePrefab;
    public Transform LaunchOffset;
    // Update is called once per frame
    void Update()
    {
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

         if (Input.GetButtonDown("Fire1")) 
        {
            Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
        }
    
    }
}
