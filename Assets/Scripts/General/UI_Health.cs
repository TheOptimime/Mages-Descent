using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour {

    public Image healthBar;
    private Health health;
    //public Fighter player;

    
    void Start()
    {
        health = GetComponent<Health>();
        healthBar.fillAmount = (health.currentHealth / health.maxHealth);
    }

    
    void Update()
    {
        healthBar.fillAmount = (health.currentHealth/health.maxHealth);
    }
}
