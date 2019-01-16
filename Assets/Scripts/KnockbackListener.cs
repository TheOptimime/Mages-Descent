using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackListener : MonoBehaviour {

    public float recoveryTime, stunLength;
    bool stunned;

    public enum KnockbackRecipient
    {
        Player,
        Enemy,
        PlayerAI
    }

    public KnockbackRecipient knockbackRecipient;

    Fighter fighter;
    EnemyAI enemyAI;
    PlayerAI playerAI;

	void Start () {

        if(knockbackRecipient == KnockbackRecipient.Player)
        {
            fighter = GetComponent<Fighter>();
        }
        else if(knockbackRecipient == KnockbackRecipient.PlayerAI)
        {
            playerAI = GetComponent<PlayerAI>();
        }
        else if(knockbackRecipient == KnockbackRecipient.Enemy)
        {
            enemyAI = GetComponent<EnemyAI>();
        }
	}
	
	void Update () {
		
	}

    public void SetHitstun(float hitstun)
    {

    }

    public void SetKnockback(Vector2 knockback)
    {
        switch (knockbackRecipient)
        {
            case KnockbackRecipient.Player:
                break;

            case KnockbackRecipient.PlayerAI:
                break;

            case KnockbackRecipient.Enemy:
                break;
        }
    }
}
