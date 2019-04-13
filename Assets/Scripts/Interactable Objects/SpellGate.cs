using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellGate : MonoBehaviour
{
    public Attack.Element element;
    public Attack.AttackPath attackPath;
    public int health;

    public bool requireElement, requireAttackPath, hasHealth;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "SpellGate";

        if(hasHealth && health == 0)
        {
            Debug.LogWarning("Health value not set. Setting to default value of 50");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHealth)
        {
            if(health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        
    }
}
