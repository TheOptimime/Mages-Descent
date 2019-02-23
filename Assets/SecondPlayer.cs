using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayer : MonoBehaviour
{

    InputHandler ih;

    // Start is called before the first frame update
    void Start()
    {
        ih = GetComponent<InputHandler>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ih.playerIndex = XInputDotNetPure.PlayerIndex.Two;
    }
}
