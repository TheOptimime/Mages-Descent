using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{

    InputHandler ih;
    float timer;
    public int sceneNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ih = collision.gameObject.GetComponent<InputHandler>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(ih != null)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (collision.gameObject.GetComponent<InputHandler>() == ih)
                {
                    if (ih.joystickPosition == 8)
                    {
                        timer += Time.deltaTime;
                    }
                }
            }

            if (timer >= 2.4f)
            {
                SceneManager.LoadScene(sceneNumber);
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ih = null;
        timer = 0;
    }
}
