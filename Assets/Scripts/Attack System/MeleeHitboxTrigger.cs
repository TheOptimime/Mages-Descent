using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxTrigger : MonoBehaviour
{
    public bool enableHitbox;

    public MeleeHitbox hitbox;

    private void Update()
    {
        if (enableHitbox && !hitbox.active)
        {
            hitbox.Activate();
        }
        else if (!enableHitbox && hitbox.active)
        {
            hitbox.DeActivate();
        }
    }
}
