using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour {

    public float maxHealth, currentHealth;
    public bool regen, degen;

    public float regenRate, degenRate;
    public float timer, time, invulnerabilityTimer;

    private TextMeshPro _tmpro;
    private int lastDmgDealt;
    public Transform numberSpawn;
    



    bool invulnerable;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        
        
        _tmpro.fontSize = 15;
       

        
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
                time = 0;
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
        if (!invulnerable)
        {
            currentHealth -= damage;
            
            //Instantiate(_tmpro, numberSpawn.position, Quaternion.identity);
            //_tmpro.text = damage.ToString();

        }
    }
}
