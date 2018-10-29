using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputHandler : MonoBehaviour {

    bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    public Fighter player;
    SpellDatabase spellDatabase;

    [Range(0.3f, 0.8f)]
    public float deadzone = 0.3f;

    public int joystickPosition;

    public List<int> joystickRecord;
    
    public Vector2 Checkpoints, joystick;
    bool buttonPressed;

    [Range(0,1)]
    public float vibrateLeftMotor, vibrateRightMotor;
    float timeBeforeLastInput, timeForNextInput;

    private void Start()
    {
        player = GetComponent<Fighter>();
        joystickRecord = new List<int>();
    }

    void Update () {
        //int directionalInput;

        FindController();

        HandleButtons();
        HandleShoulderButtons();
        HandleDPad();

        HandleJoystick(out joystickPosition);


        //Vector2 pad = new Vector2((int)(state.DPad.Left) * -1 + (int)(state.DPad.Right), (int)(state.DPad.Down) * -1 + (int)(state.DPad.Up));

        // print(joystickRecord.Count);


            if (joystickRecord.Count == 0 && timeBeforeLastInput == 0 && joystickPosition != 5 )
            {
                joystickRecord.Add(joystickPosition);
                timeBeforeLastInput = Time.time;
                timeForNextInput = timeBeforeLastInput + 0.3f;
                print("joystick input detected: " + joystickPosition);
            }
            else if (Time.time > timeForNextInput)
            {
                joystickRecord.Clear();
                timeBeforeLastInput = 0;
                //print("joystick record cleared");
            }
            else if(Time.time < timeForNextInput && joystickPosition != joystickRecord[joystickRecord.Count - 1])
            {
                joystickRecord.Add(joystickPosition);
                timeBeforeLastInput = Time.time;
                timeForNextInput = timeBeforeLastInput + 0.3f;
                print("joystick input detected");
            }
            else if (buttonPressed)
            {
            print("Button Pressed, Input Cleared");
                joystickRecord.Clear();
            }

        //print(joystickRecord);
        //print(joystickPosition);
        
        prevState = state;
        state = GamePad.GetState(playerIndex);
        
    }

    private void FixedUpdate()
    {
        // Experimental Vibration Control
        
        GamePad.SetVibration(playerIndex, vibrateLeftMotor, vibrateRightMotor);
        
    }

    void HandleButtons()
    {
        List<int> frozenJoystickRecord = new List<int>();

        #region A Button
        if(prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
//            print("A Button Pressed");
            frozenJoystickRecord = joystickRecord;

            for(int i = 0; i < frozenJoystickRecord.Count; i++)
            {
                print("frozen input " + i + " : " + frozenJoystickRecord[i]);
            }

            print("FrozenJoystickRecord: " + frozenJoystickRecord.Count);
            

            for(int i = 0; i < player.moveset.spellBook_A_Button.attacks.Count; i++)
            {
                int matchCount = 0;
                

                if(player.moveset.spellBook_A_Button.attacks[i].joystickCommand != null && player.moveset.spellBook_A_Button.attacks[i].joystickCommand.Count == frozenJoystickRecord.Count)
                {
                    for(int joystickNumber = 0; joystickNumber < player.moveset.spellBook_A_Button.attacks[i].joystickCommand.Count; joystickNumber++)
                    {
                        print(player.moveset.spellBook_A_Button.attacks[i].joystickCommand[joystickNumber] + " : " + frozenJoystickRecord[joystickNumber]);

                        if(player.moveset.spellBook_A_Button.attacks[i].joystickCommand[joystickNumber] == frozenJoystickRecord[joystickNumber])
                        {
                            
                            matchCount++;
                            print("matchfound: " + matchCount);
                        }

                    }
                }

                if(matchCount == player.moveset.spellBook_A_Button.attacks[i].joystickCommand.Count)
                {
                    print("match found");
                }
                

                foreach (int joystickNumber in frozenJoystickRecord)
                {
                  //  print("playerInputRecord: " + joystickNumber);
                }

                //print("loop running");
                if(InputCompare(player.moveset.spellBook_A_Button.attacks[i].joystickCommand, frozenJoystickRecord))
                {
                    //print("command found");
                    player.SetAttackQueue(player.moveset.spellBook_A_Button.attacks[i]);
                    break;
                }
                else if(player.moveset.spellBook_A_Button.attacks[i].joystickCommand == null || i >= player.moveset.spellBook_A_Button.attacks.Count)
                {
                    // this is the generic attack
                    //print("generic attack");
                    player.SetAttackQueue(player.moveset.spellBook_A_Button.attacks[i]);
                    break;
                }
            }

            

        }
        else if(prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Pressed)
        {
            print("A Button Held");
            
            player.RelayButtonInput();
        }
        else if(prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
        {
            //print("A Button Released");
            player.OnAttackButtonRelease();
        }
        #endregion

        #region B Button
        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
        {
            //print("B Button Pressed");
        }
        else if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Pressed)
        {
            //print("B Button Held");
        }
        else if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released)
        {
            //print("B Button Released");
        }
        #endregion

        #region X Button
        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
        {
            //print("X Button Pressed");
        }
        else if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Pressed)
        {
            //print("X Button Held");
        }
        else if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released)
        {
            //print("X Button Released");
        }
        #endregion

        #region Y Button
        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            //print("Y Button Pressed");
        }
        else if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Pressed)
        {
            //print("Y Button Held");
        }
        else if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released)
        {
            //print("Y Button Released");
        }
        #endregion

        #region L Button
        if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            //print("L Button Pressed");
        }
        else if (prevState.Buttons.LeftShoulder == ButtonState.Pressed && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            //print("L Button Held");
        }
        else if (prevState.Buttons.LeftShoulder == ButtonState.Pressed && state.Buttons.LeftShoulder == ButtonState.Released)
        {
            //print("L Button Released");
        }
        #endregion

        #region R Button
        if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
        {
            //print("R Button Pressed");
        }
        else if (prevState.Buttons.RightShoulder == ButtonState.Pressed && state.Buttons.RightShoulder == ButtonState.Pressed)
        {
            //print("R Button Held");
        }
        else if (prevState.Buttons.RightShoulder == ButtonState.Pressed && state.Buttons.RightShoulder == ButtonState.Released)
        {
            //print("R Button Released");
        }
        #endregion

        frozenJoystickRecord = null;
    }

    void HandleShoulderButtons()
    {
        #region L2 Button
        if (prevState.Triggers.Left <= 0.2f && state.Triggers.Left >= 0.3f)
        {
            print("L2 Button Pressed");
        }
        else if (prevState.Triggers.Left >= 0.3f && state.Triggers.Left >= 0.3f)
        {
            print("L2 Button Held");
        }
        else if (prevState.Triggers.Left >= 0.3f && state.Triggers.Left <= 0.2f)
        {
            print("L2 Button Released");
        }
        #endregion

        #region R2 Button
        if (prevState.Triggers.Right <= 0.2f && state.Triggers.Right >= 0.3f)
        {
            print("R2 Button Pressed");
        }
        else if (prevState.Triggers.Right >= 0.3f && state.Triggers.Right >= 0.3f)
        {
            print("R2 Button Held");
        }
        else if (prevState.Triggers.Right >= 0.3f && state.Triggers.Right <= 0.2f)
        {
            print("R2 Button Released");
        }
        #endregion
    }

    void HandleDPad()
    {
        if(state.DPad.Up == ButtonState.Pressed || state.DPad.Down == ButtonState.Pressed)
        {
            if(state.DPad.Up == ButtonState.Pressed)
            {
                #region DPad_Left
                if (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed)
                {
                    //print("Left Button pressed");
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Pressed)
                {
                    //print("Left Button Held");
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Released)
                {
                    //print("Left Button Released");
                }
                #endregion

                #region DPad_Right
                if (prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed)
                {
                    //print("Right Button pressed");
                }
                else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Pressed)
                {
                    //print("Right Button Held");
                }
                else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Released)
                {
                    //print("Right Button Released");
                }
                #endregion
            }
            else if(state.DPad.Down == ButtonState.Pressed)
            {
                #region DPad_Left
                if (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed)
                {
                    //print("Left Button pressed");
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Pressed)
                {
                    //print("Left Button Held");
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Released)
                {
                    //print("Left Button Released");
                }
                #endregion

                #region DPad_Right
                if (prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed)
                {
                    print("Right Button pressed");
                }
                else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Pressed)
                {
                    print("Right Button Held");
                }
                else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Released)
                {
                    print("Right Button Released");
                }
                #endregion
            }
        }
        else
        {
            // neither up or down are being pressed

            #region DPad_Left
            if (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed)
            {
                print("Left Button pressed");
            }
            else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Pressed)
            {
                print("Left Button Held");
            }
            else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Released)
            {
                print("Left Button Released");
            }
            #endregion

            #region DPad_Right
            if (prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed)
            {
                //print("Right Button pressed");
            }
            else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Pressed)
            {
                //print("Right Button Held");
            }
            else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Released)
            {
                //print("Right Button Released");
            }
            #endregion
        }



        #region DPad_Up
        if (prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed)
        {
            //print("Up Button pressed");
        }
        else if (prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up == ButtonState.Pressed)
        {
            //print("Up Button Held");
        }
        else if (prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up == ButtonState.Released)
        {
            //print("Up Button Released");
        }
        #endregion

        #region DPad_Down
        if (prevState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed)
        {
            //print("Down Button pressed");
        }
        else if (prevState.DPad.Down == ButtonState.Pressed && state.DPad.Down == ButtonState.Pressed)
        {
            //print("Down Button Held");
        }
        else if (prevState.DPad.Down == ButtonState.Pressed && state.DPad.Down == ButtonState.Released)
        {
            //print("Down Button Released");
        }
        #endregion

      
    }

    void HandleJoystick(out int directionalInput)
    {
        joystick = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        Vector2 joystickRaw = Vector2.zero;

        joystickRaw.x = joystick.x > deadzone || joystick.x < -deadzone ? Mathf.Sign(joystick.x) : 0;
        joystickRaw.y = joystick.y > deadzone || joystick.y < -deadzone ? Mathf.Sign(joystick.y) : 0;

        //print("X: " + joystickRaw.x + " Y: " + joystickRaw.y);

        //int joystickPosition = (int)((joystickRaw.x + 2) + (-joystickRaw.y + 3));

        directionalInput = (int)((joystickRaw.x + 2) + (joystickRaw.y == 1 ? -joystickRaw.y + 1 : joystickRaw.y == -1 ? joystickRaw.y + 7 : 3));

        
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
                    //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }
    }

    static bool InputCompare(List<int> a, List<int> b)
    {
        if (a == null) return b == null;
        if (b == null || a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (!object.Equals(a[i], b[i]))
            {
                return false;
            }
        }
        return true;
    }
}
