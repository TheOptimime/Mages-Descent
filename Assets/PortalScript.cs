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
        if(collision.gameObject.tag == "Player")
        {
            print("enter");
            ih = collision.gameObject.GetComponent<InputHandler>();
            print(ih);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            print("staying");
            if (ih == null)
            {
                ih = collision.gameObject.GetComponent<InputHandler>();
            }
        }
        
        
    }

    private void Update()
    {
        if (ih != null)
        {
            if (ih.joystickPosition == 8)
            {
                timer += Time.deltaTime;
            }

            if (timer >= 0.4f)
            {
                SceneManager.LoadScene(sceneNumber);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            print("exited");
            ih = null;
            timer = 0;
        }
        
    }
}
