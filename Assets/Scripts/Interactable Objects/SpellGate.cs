using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class SpellGate : MonoBehaviour
{
    public Attack.Element element;
    public Attack.AttackPath attackPath;
    public Health health;

    public bool requireElement, requireAttackPath, hasHealth;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "SpellGate";

        if(hasHealth && health.maxHealth == 0)
        {
            Debug.LogWarning("Health value not set. Setting to default value of 50");
            health.currentHealth = health.maxHealth = 50;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHealth)
        {
            if(health.currentHealth <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        
    }
}
