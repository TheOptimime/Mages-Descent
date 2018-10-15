using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputHandler : MonoBehaviour {

    bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public int joystickPosition;

    public List<int> joystickRecord;
    
    public Vector2 Checkpoints, joystick;
    bool isListening;

    [Range(0,1)]
    public float vibrateLeftMotor, vibrateRightMotor;
    float timeBeforeLastInput, timeForNextInput;

	void Update () {
        joystick = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        Vector2 joystickRaw = Vector2.zero;

        joystickRaw.x = joystick.x > 0.5f || joystick.x < -0.5f ? Mathf.Sign(joystick.x) : 0;
        joystickRaw.y = joystick.y > 0.5f || joystick.y < -0.5f ? Mathf.Sign(joystick.y) : 0;

        print("X: " + joystickRaw.x + " Y: " + joystickRaw.y);

        //int joystickPosition = (int)((joystickRaw.x + 2) + (-joystickRaw.y + 3));

        joystickPosition = (int)((joystickRaw.x + 2) + (joystickRaw.y == 1? -joystickRaw.y + 1 : joystickRaw.y == -1 ? joystickRaw.y + 7 : 3));


        

        //Vector2 pad = new Vector2((int)(state.DPad.Left) * -1 + (int)(state.DPad.Right), (int)(state.DPad.Down) * -1 + (int)(state.DPad.Up));

        if (timeBeforeLastInput > timeForNextInput && joystickPosition != joystickRecord[joystickRecord.Count-1])
        {
            joystickRecord.Add(joystickPosition);
        }
        else
        {

        }

        
        print(joystickPosition);

        FindController();

        prevState = state;
        state = GamePad.GetState(playerIndex);

        

        
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
