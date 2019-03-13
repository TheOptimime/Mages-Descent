using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour {

    public Image healthBar;
    private Health health;
    public Fighter player;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        healthBar.fillAmount = (health.maxHealth/100);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (health.currentHealth/100);
    }
}
