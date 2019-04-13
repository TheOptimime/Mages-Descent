using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackListener : MonoBehaviour {

    public float recoveryTime, stunLength;
    bool stunned;

    public enum KnockbackRecipient
    {
        Player,
        AI
    }

    public KnockbackRecipient knockbackRecipient;

    Fighter _fighter;
    AI _ai;

	void Start () {

        if(knockbackRecipient == KnockbackRecipient.Player)
        {
            _fighter = GetComponent<Fighter>();
        }
        else if(knockbackRecipient == KnockbackRecipient.AI)
        {
            _ai = GetComponent<AI>();
        }
	}

    public void SetHitstun(DoubleTime hitstun)
    {
        switch (knockbackRecipient)
        {
            case KnockbackRecipient.Player:
                {
                    _fighter.SetHitstunTimer(hitstun);
                }
                break;

            case KnockbackRecipient.AI:
                {
                    _ai.SetHitstunTimer(hitstun);
                }

                break;
        }
    }

    public void SetKnockback(Vector2 knockback)
    {
        switch (knockbackRecipient)
        {
            case KnockbackRecipient.Player:
                {
                    print("Knocking back player: " + knockback);
                    _fighter.rb.velocity = Vector2.zero;
                    //_fighter.cc.m_Velocity = knockback;
                    print(_fighter.cc.m_Velocity);
                    //_fighter.rb.AddForce(knockback, ForceMode2D.Impulse);
                }
                break;

            case KnockbackRecipient.AI:
                {
                    _ai.rb2d.velocity = Vector2.zero;
                    _ai.rb2d.AddForce(knockback, ForceMode2D.Impulse);
                }
                
                break;
        }
    }
}
