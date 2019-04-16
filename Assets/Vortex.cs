using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            float distance = transform.position.x - collision.transform.position.x;
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            enemy.lockMovement = true;
            enemy.ec.Move(distance, false, false);
            
        }
    }
}
