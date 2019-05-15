using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDamage : MonoBehaviour
{
    public EnemyAI enemy;
    public int damage;
    public Vector2 knockback;
    public DoubleTime hitstun;

    private void Start()
    {
        if (enemy == null) enemy = gameObject.gameObject.GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            print("Damaging player");
            
            collision.gameObject.GetComponent<Health>().Damage(damage);
            collision.gameObject.GetComponent<Fighter>().recentlyAttacked = true;
            if(enemy) enemy.hitTarget = true;
            //print(collision.gameObject.GetComponent<Health>().currentHealth);
            KnockbackListener kbl = collision.gameObject.GetComponent<KnockbackListener>();

            kbl.SetKnockback(knockback);
            kbl.SetHitstun(hitstun);
            
        }
    }
}
