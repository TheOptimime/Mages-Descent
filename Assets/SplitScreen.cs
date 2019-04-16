using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreen : MonoBehaviour
{
    public Camera player2Cam;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("displays connected: " + Display.displays.Length);

        if (Display.displays.Length > 1)
        {
            player2Cam.targetDisplay = 1;
            Display.displays[1].Activate();
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
