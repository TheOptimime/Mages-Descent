using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    // This script is attatched to attacks like projectiles

    public Vector2 speed;

    Attack attack;

    Rigidbody2D rb;
    public float delay;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        Destroy(this.gameObject, delay);
    }
    
    void Update()
    {
        rb.velocity = speed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Modt likely going to make a tag for interactable ojbects

        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().Damage(attack.damage);
        }

    }
}
