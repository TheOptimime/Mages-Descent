using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHealth, currentHealth;
    public bool regen, degen;

    public float regenRate, degenRate;
    public float timer, time, invulnerabilityTimer;

    bool invulnerable;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {

        if(regen && degen)
        {
            regen = degen = false;
        }

        if (regen)
        {
            time++;
            if(time % regenRate == 0)
            {
                currentHealth++;
            }
        }
        else if (degen)
        {
            time++;
            if(time % degenRate == 0)
            {
                currentHealth--;
            }
        }

        if(invulnerabilityTimer > 0)
        {
            invulnerable = true;
        }
        else
        {
            invulnerable = false;
        }

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
		
	}

    public void Damage(int damage)
    {
        if (invulnerable)
        {
            currentHealth -= damage;
        }
    }
}
