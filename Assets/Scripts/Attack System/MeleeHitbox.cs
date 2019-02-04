using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour {

    Attack properties;
    float time;
    Collider2D collider;
    bool active;
    
    public MeleeHitbox(Attack baseAttack)
    {
        properties.attackType = baseAttack.attackType;
        properties.element = baseAttack.element;
        properties.attackLength = baseAttack.attackLength;
    }

    public MeleeHitbox(Attack.AttackType attackType, Attack.Element element, float length)
    {
        properties.attackType = attackType;
        properties.element = element;
        properties.attackLength = length;
    }

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
        DeActivate();
    }

    private void Update()
    {
        if (active)
        {
            time += Time.deltaTime;

            if (time >= properties.lifetime)
            {
                DeActivate();
            }
        }
        
    }



    public void Activate()
    {
        time = 0;

        if(properties != null)
        {
            collider.enabled = true;
        }

        active = true;
    }

    public void DeActivate()
    {
        // Disable object and reset data
        collider.enabled = false;
        properties = null;
        active = false;
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

}
