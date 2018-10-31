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

    public float time, timer;

    int castingPlayer;

    void Start()
    {
        print("attack start");
        transform.position = origin;
        rb = gameObject.AddComponent<Rigidbody2D>();
        col = gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        testSprite = Resources.Load<Sprite>(attack.spritePath);
        print(testSprite);
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = testSprite;

        
        
        sr.sprite = Resources.Load<Sprite>("fireball.png");
        print("attack init complete");
    }
    
    void Update()
    {
        timer += Time.deltaTime;

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
        else if (attack.name == "Dab Ice")
        {
            sr.color = new Color(0.16f, 0.2f, 0.56f);
        }


        //print(gameObject.name);

        if (time > attack.lifetime)
        {
            // destroy this object

        }
    }

    private void FixedUpdate()
    {
        if (attack != null)
        {
            //print("yeet");
            if(attack.name == "Yeet Fire")
            {
                rb.velocity = new Vector2(attack.speed * direction, 0) * Mathf.Sin(Time.deltaTime * 20) * 0.5f;
            }
            else
            {
                rb.velocity = new Vector2(attack.speed * direction, 0);
            }
            
            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
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
