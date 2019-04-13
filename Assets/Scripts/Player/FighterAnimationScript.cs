using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimationScript : MonoBehaviour
{
    public PlayerController2D cc;
    public Animator anim;
    public Fighter fighter;

    private void Update()
    {
        
    }

    private void LateUpdate()
    {

        if (!fighter.isDead)
        {
            if (cc.m_Grounded)
            {
                anim.SetInteger("State", 0);
            }
            else if (cc.m_Rigidbody2D.velocity.y < 0 && !cc.m_Grounded)
            {
                anim.SetInteger("State", 2);
            }
            else if (cc.m_Rigidbody2D.velocity.y > 0)
            {
                anim.SetInteger("State", 1);
            }




            if (fighter.horizontalMove == 0)
            {
                anim.SetFloat("speed", 0);
            }
            else
            {
                anim.SetFloat("speed", 1);
            }

            if (fighter.justLanded)
            {
                anim.SetBool("isJumping", false);
            }
        }
        else
        {
            //anim.SetTrigger("death");
        }
        
        
    }
}
