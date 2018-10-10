using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    // This script is attatched to attacks like projectiles

    //public Vector2 speed;

    public Attack attack;

    Rigidbody2D rb;
    public float delay;

    //public Sprite[] projectileSprites;
    public Sprite testSprite;
    CircleCollider2D col;

    SpriteRenderer sr;

    public Vector2 origin;
    public int direction;

    int castingPlayer;

    void Start()
    {
        print("attack start");
        transform.position = origin;
        rb = gameObject.AddComponent<Rigidbody2D>();
        col = gameObject.AddComponent<CircleCollider2D>();
        testSprite = Resources.Load<Sprite>(attack.spritePath);
        print(testSprite);
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = testSprite;
        /*
        if (attack != null)
        {
            projectileSprites = new Sprite[0];
            projectileSprites = Resources.LoadAll<Sprite>("fireball.png");
            if(projectileSprites == null)
            {
                print("no projectile loaded");
            }
            else
            {
                print("sprite loaded");
            }
            //sr.sprite
        }
        else
        {
            print("attack is still null");
        }
        */
        //Destroy(this.gameObject, delay);
        sr.sprite = Resources.Load<Sprite>("fireball.png");
        print("attack init complete");
    }
    
    void Update()
    {
        //testSprite = Resources.Load<Sprite>("fireball.png");
        if(sr.sprite == null)
        {
            sr.sprite = testSprite;
        }

        if(attack.name == "Triple Fire")
        {
            sr.color = new Color(1, 0, 0);
        }
        else if(attack.name == "Dark Fire")
        {
            sr.color = new Color(0.28f, 0f, 0.28f);
        }
        
        
        print(gameObject.name);
        
        if(attack != null )
        {
            print("yeet");
            rb.velocity = new Vector2(attack.speed * direction, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Most likely going to make a tag for interactable ojbects

        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().Damage(attack.damage);
            Destroy(gameObject);
        }
    }

    public void SetProjectile(Attack _attack)
    {
        attack = _attack;
    }
}
