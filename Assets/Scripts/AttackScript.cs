using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    // This script is attatched to attacks like projectiles

    //public Vector2 speed;

    public Attack attack;
    public string user;
    Rigidbody2D rb;
    public float delay, defaultLifeSpan;

    //public Sprite[] projectileSprites;
    public Sprite testSprite;
    CircleCollider2D col;

    SpriteRenderer sr;
    Sprite[] sprites;

    public Vector2 origin;
    public int direction;
    public bool flipped;

    public float time;

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
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        sr.transform.localScale += new Vector3(3,3,3);
        gameObject.tag = "Attack";
        sr.sortingOrder = 8;
        sprites = Resources.LoadAll<Sprite>("fireball.png");
        sr.sprite = Resources.Load<Sprite>("fireball.png");
        print("attack init complete");
       
    }
    
    void Update()
    {
        time += Time.deltaTime;
        
            sr.flipX = flipped;
        

        
        if(sr.sprite == null)
        {
            sr.sprite = testSprite;
        }

        if(attack.name == "Triple Fire")
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                sr.sprite = sprites[i];

                
                

            }
            
            
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

        if (time > attack.lifetime || time > defaultLifeSpan)
        {
            // destroy this object
            if(attack.lifetime == 0 && time > defaultLifeSpan)
            {
                Destroy(gameObject);
            }
            else if (time > attack.lifetime)
            {
                Destroy(gameObject);
            }

        }

        
    }

    private void FixedUpdate()
    {
        if (attack != null)
        {
            print("attack is not null");
            //print("yeet");
            if(attack.name == "Yeet Fire")
            {
                rb.velocity = new Vector2(attack.speed * direction, 0) * Mathf.Sin(Time.fixedDeltaTime * 20) * 0.5f;
            }
            else
            {
                print("Should be moving: " + direction);
                rb.velocity = new Vector2(attack.speed * direction * Time.deltaTime * 20, 0);
                
            }
            
            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Most likely going to make a tag for interactable ojbects

        if (other.transform.tag == "Enemy" && other.gameObject.name != user || other.transform.tag == "Player" && other.gameObject.name != user)
        {
            other.gameObject.GetComponent<Health>().Damage(attack.damage);
            

            if(other.transform.tag == "Player")
            {
                Fighter player = other.gameObject.GetComponent<Fighter>();
                player.recentlyAttacked = true;
                player.recoveryTimer = attack.hitStun;
            }
            else if(other.transform.tag == "Enemy")
            {
                EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
                enemy.hit = true;
                enemy.hitTimer = attack.hitStun;
            }

            Destroy(gameObject);

        }
        else if(other.transform.tag == "Ground" || other.transform.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    

    public void SetProjectile(Attack _attack)
    {
        attack = _attack;
    }
}
