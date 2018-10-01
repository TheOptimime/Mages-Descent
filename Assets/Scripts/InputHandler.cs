using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputHandler : MonoBehaviour {

    bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    Vector2 Checkpoints;
    bool isListening;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        FindController();

        prevState = state;
        state = GamePad.GetState(playerIndex);

        if(state.Triggers.Left == 0)
        {
            print("no");
            
        }
        else
        {
            GamePad.SetVibration(playerIndex, state.Triggers.Left * 0.2f, 0.4f);
            print("yeet");
        }

        if (BeginListening() && isListening == false)
        {
            float timer = Time.deltaTime;



        }
	}

    private void FixedUpdate()
    {
        // Experimental Vibration Control

        if(Input.GetKey(KeyCode.Space))
        GamePad.SetVibration(playerIndex, 0.2f, 0.4f);
        
    }

    bool BeginListening()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && isListening == false)
        {
            return true;
        }
        return false;
    }

    void FindController()
    {
        // Temporary, At this stage we may want to set this manually.

        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i< 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }
    }
}
