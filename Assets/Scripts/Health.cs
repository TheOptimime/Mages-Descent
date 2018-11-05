using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int maxHealth, currentHealth;
    public bool regen;

    public float timer, time;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (regen)
        {
            time++;
            if(time % 500 == 0)
            {
                currentHealth++;
            }
        }
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
		
	}

    public void Damage(int damage)
    {
        currentHealth -= damage;
    }
}
