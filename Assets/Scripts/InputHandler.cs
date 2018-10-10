using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputHandler : MonoBehaviour {

    bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public Vector2 Checkpoints, joystick;
    bool isListening;

    [Range(0,1)]
    public float vibrateLeftMotor, vibrateRightMotor;


	void Update () {

        joystick = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);

        Vector2 pad = new Vector2((int)(state.DPad.Left) * -1 + (int)(state.DPad.Right), (int)(state.DPad.Down) * -1 + (int)(state.DPad.Up));

        if (joystick.x != 0)
        {
            //print("X: " + state.ThumbSticks.Left.X);
        }
        
        if(joystick.y != 0)
        {
            //print("Y: " + state.ThumbSticks.Left.Y);
        }

        int joystickPosition =(int)((joystick.x + 2) + (-joystick.y + 3));
        print(joystickPosition);

        FindController();

        prevState = state;
        state = GamePad.GetState(playerIndex);

        
        if (BeginListening() && isListening == false)
        {
            float timer = Time.deltaTime;
        }

        
    }

    private void FixedUpdate()
    {
        // Experimental Vibration Control
        
        GamePad.SetVibration(playerIndex, vibrateLeftMotor, vibrateRightMotor);
        
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
