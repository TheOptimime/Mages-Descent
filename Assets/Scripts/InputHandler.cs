﻿using System.Collections;
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

    List<List<int>> ComboInputNormal, ComboInputReverse;

    public bool keyboardInputInUse, dPadInputInUse;

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
        InitializeJoyStickCommands();
    }

    void Update () {
        //int directionalInput;
        if(playerIndexSet != true)
        {
            FindController();
        }
        


        HandleButtons();

        HandleShoulderButtons();

        if (keyboardInputInUse)
        {

        }
        else if (dPadInputInUse)
        {
            HandleDPad(out joystickPosition);
        }
        else
        {
            HandleJoystick(out joystickPosition);
        }
        

        


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

            //print(quarterCircleDownRight);
            
            for(int i = 0; i < ComboInputNormal[0].Count; i++)
            {
                int matchCount = 0;
                

                if(ComboInputNormal[0].Count == frozenJoystickRecord.Count)
                {
                    for(int joystickNumber = 0; joystickNumber < ComboInputNormal[0].Count; joystickNumber++)
                    {
                        
                        if(ComboInputNormal[0][joystickNumber] == frozenJoystickRecord[joystickNumber])
                        {
                            matchCount++;
                            print("matchfound: " + matchCount);
                        }

                    }
                }
                
            }

            player.jump = true;

        }
        else if(prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Pressed)
        {
            print("A Button Held");
            
            player.RelayJumpButtonInput();
        }
        else if(prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
        {
            //print("A Button Released");
            
        }
        #endregion

        #region B Button
        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
        {
            // B button is pressed
            // Creates a record of the joystick inputs on button press
            frozenJoystickRecord = joystickRecord;
            
            // Checking if the inputs entered matched any of the spells
            for (int i = 0; i < player.moveset.spellBook_B_Button.attacks.Count; i++)
            {
                int matchCount = 0;

                // Makes a temporary copy of this joystick command
                List<int> tempJoystickCommand = new List<int>();
                

                if (!player.controller.m_FacingRight)
                {
                    FlipInput(player.moveset.spellBook_B_Button.attacks[i].joystickCommand, out tempJoystickCommand);
                }
                else
                {
                    tempJoystickCommand = player.moveset.spellBook_B_Button.attacks[i].joystickCommand;
                }

                if(tempJoystickCommand != null)
                {
                    if (tempJoystickCommand.Count == frozenJoystickRecord.Count)
                    {
                        for (int joystickNumber = 0; joystickNumber < tempJoystickCommand.Count; joystickNumber++)
                        {
                            print(tempJoystickCommand[joystickNumber] + " : " + frozenJoystickRecord[joystickNumber]);

                            if (tempJoystickCommand[joystickNumber] == frozenJoystickRecord[joystickNumber])
                            {
                                // increments value if a matched input is found
                                matchCount++;
                                print("matchfound: " + matchCount);
                            }

                        }
                    }

                    // Compares
                    if (matchCount == tempJoystickCommand.Count)
                    {
                        player.SetAttackQueue(player.moveset.spellBook_B_Button.attacks[i]);
                        break;
                    }
                    
                }
                else if (player.moveset.spellBook_B_Button.attacks[i].joystickCommand == null || i >= player.moveset.spellBook_B_Button.attacks.Count)
                {
                    // this is the generic attack
                    player.SetAttackQueue(player.moveset.spellBook_B_Button.attacks[i]);
                    break;
                }

            }



        }
        else if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Pressed)
        {
            print("B Button Held");

            player.RelayButtonInput();
        }
        else if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released)
        {
            //print("B Button Released");
            player.OnAttackButtonRelease();
        }
        #endregion

        #region X Button
        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
        {
            // B button is pressed
            // Creates a record of the joystick inputs on button press
            frozenJoystickRecord = joystickRecord;

            // Checking if the inputs entered matched any of the spells
            for (int i = 0; i < player.moveset.spellBook_X_Button.attacks.Count; i++)
            {
                int matchCount = 0;

                // Makes a temporary copy of this joystick command
                List<int> tempJoystickCommand = new List<int>();


                if (!player.controller.m_FacingRight)
                {
                    FlipInput(player.moveset.spellBook_X_Button.attacks[i].joystickCommand, out tempJoystickCommand);
                }
                else
                {
                    tempJoystickCommand = player.moveset.spellBook_X_Button.attacks[i].joystickCommand;
                }

                if (tempJoystickCommand != null)
                {
                    if (tempJoystickCommand.Count == frozenJoystickRecord.Count)
                    {
                        for (int joystickNumber = 0; joystickNumber < tempJoystickCommand.Count; joystickNumber++)
                        {
                            print(tempJoystickCommand[joystickNumber] + " : " + frozenJoystickRecord[joystickNumber]);

                            if (tempJoystickCommand[joystickNumber] == frozenJoystickRecord[joystickNumber])
                            {
                                // increments value if a matched input is found
                                matchCount++;
                                print("matchfound: " + matchCount);
                            }

                        }
                    }

                    // Compares
                    if (matchCount == tempJoystickCommand.Count)
                    {
                        player.SetAttackQueue(player.moveset.spellBook_X_Button.attacks[i]);
                        break;
                    }

                }
                else if (player.moveset.spellBook_X_Button.attacks[i].joystickCommand == null || i >= player.moveset.spellBook_X_Button.attacks.Count)
                {
                    // this is the generic attack
                    player.SetAttackQueue(player.moveset.spellBook_X_Button.attacks[i]);
                    break;
                }

            }



        }
        else if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Pressed)
        {
            print("X Button Held");

            player.RelayButtonInput();
        }
        else if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released)
        {
            //print("X Button Released");
            player.OnAttackButtonRelease();
        }
        #endregion

        #region Y Button
        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            // B button is pressed
            // Creates a record of the joystick inputs on button press
            frozenJoystickRecord = joystickRecord;

            // Checking if the inputs entered matched any of the spells
            for (int i = 0; i < player.moveset.spellBook_Y_Button.attacks.Count; i++)
            {
                int matchCount = 0;

                // Makes a temporary copy of this joystick command
                List<int> tempJoystickCommand = new List<int>();


                if (!player.controller.m_FacingRight)
                {
                    FlipInput(player.moveset.spellBook_Y_Button.attacks[i].joystickCommand, out tempJoystickCommand);
                }
                else
                {
                    tempJoystickCommand = player.moveset.spellBook_Y_Button.attacks[i].joystickCommand;
                }

                if (tempJoystickCommand != null)
                {
                    if (tempJoystickCommand.Count == frozenJoystickRecord.Count)
                    {
                        for (int joystickNumber = 0; joystickNumber < tempJoystickCommand.Count; joystickNumber++)
                        {
                            print(tempJoystickCommand[joystickNumber] + " : " + frozenJoystickRecord[joystickNumber]);

                            if (tempJoystickCommand[joystickNumber] == frozenJoystickRecord[joystickNumber])
                            {
                                // increments value if a matched input is found
                                matchCount++;
                                print("matchfound: " + matchCount);
                            }

                        }
                    }

                    // Compares
                    if (matchCount == tempJoystickCommand.Count)
                    {
                        player.SetAttackQueue(player.moveset.spellBook_Y_Button.attacks[i]);
                        break;
                    }

                }
                else if (player.moveset.spellBook_Y_Button.attacks[i].joystickCommand == null || i >= player.moveset.spellBook_Y_Button.attacks.Count)
                {
                    // this is the generic attack
                    player.SetAttackQueue(player.moveset.spellBook_Y_Button.attacks[i]);
                    break;
                }

            }



        }
        else if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Pressed)
        {
            print("Y Button Held");

            player.RelayButtonInput();
        }
        else if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released)
        {
            //print("Y Button Released");
            player.OnAttackButtonRelease();
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

    void HandleDPad(out int directionalInput)
    {
        if (state.DPad.Up == ButtonState.Pressed || state.DPad.Down == ButtonState.Pressed)
        {
            if (state.DPad.Up == ButtonState.Pressed)
            {
                #region DPad_Left
                if (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed)
                {
                    //print("Left Button pressed");
                    directionalInput = 1;
                    return;
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Pressed)
                {
                    //print("Left Button Held");
                    directionalInput = 1;
                    return;
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Released)
                {
                    //print("Left Button Released");
                    directionalInput = 1;
                    return;
                }
                #endregion

                #region DPad_Right
                if (prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed)
                {
                    //print("Right Button pressed");
                    directionalInput = 3;
                    return;
                }
                else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Pressed)
                {
                    //print("Right Button Held");
                    directionalInput = 3;
                    return;
                }
                else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Released)
                {
                    //print("Right Button Released");
                    directionalInput = 3;
                    return;
                }
                #endregion
            }
            else if (state.DPad.Down == ButtonState.Pressed)
            {
                #region DPad_Left
                if (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed)
                {
                    //print("Left Button pressed");
                    directionalInput = 7;
                    return;
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Pressed)
                {
                    //print("Left Button Held");
                    directionalInput = 7;
                    return;
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Released)
                {
                    //print("Left Button Released");
                    directionalInput = 7;
                    return;
                }
                #endregion

                #region DPad_Right
                if (prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed)
                {
                    print("Right Button pressed");
                    directionalInput = 9;
                    return;
                }
                else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Pressed)
                {
                    print("Right Button Held");
                    directionalInput = 9;
                    return;
                }
                else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Released)
                {
                    print("Right Button Released");
                    directionalInput = 9;
                    return;
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
                directionalInput = 4;
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
                directionalInput = 6;
            }
            else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Pressed)
            {
                //print("Right Button Held");
                directionalInput = 6;
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
            directionalInput = 2;
            return;
        }
        else if (prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up == ButtonState.Pressed)
        {
            //print("Up Button Held");
            directionalInput = 2;
            return;
        }
        else if (prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up == ButtonState.Released)
        {
            //print("Up Button Released");
            directionalInput = 2;
            return;
        }
        #endregion

        #region DPad_Down
        if (prevState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed)
        {
            //print("Down Button pressed");
            directionalInput = 8;
            return;
        }
        else if (prevState.DPad.Down == ButtonState.Pressed && state.DPad.Down == ButtonState.Pressed)
        {
            //print("Down Button Held");
            directionalInput = 8;
            return;
        }
        else if (prevState.DPad.Down == ButtonState.Pressed && state.DPad.Down == ButtonState.Released)
        {
            //print("Down Button Released");
            directionalInput = 8;
            return;
        }
        #endregion

        directionalInput = 5;
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

    void CheckForInputOverrides()
    {
        if (Input.anyKey)
        {
            keyboardInputInUse = true;
        }
        else if (state.DPad.Up == ButtonState.Pressed || state.DPad.Down == ButtonState.Pressed || state.DPad.Left == ButtonState.Pressed || state.DPad.Right == ButtonState.Pressed)
        {
            dPadInputInUse = true;
        }
    }

    void FlushInputOverrides()
    {
        dPadInputInUse = false;
        keyboardInputInUse = false;
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

    void InitializeJoyStickCommands()
    {
        ComboInputNormal = new List<List<int>>
        {
            new List<int>() { 8, 9, 6 },
            new List<int>() { 6, 9, 8 },
            new List<int>() { 6, 3, 2 },
            new List<int>() { 2, 3, 6 },

            new List<int>() { 6, 9, 8, 7, 4 },
            new List<int>() { 4, 1, 2, 3, 6 },
            new List<int>() { 8, 9, 6, 3, 2 },
            new List<int>() { 2, 3, 6, 9, 8 },

            new List<int>() { 6, 3, 2, 1, 4, 7, 8, 9, 6 },
            new List<int>() { 6, 9, 8, 7, 4, 1, 2, 3, 6 },

            new List<int>() { 2, 3, 6, 9 },
            new List<int>() { 8, 9, 6, 3 },
        };

        ComboInputReverse = new List<List<int>>
        {
            new List<int>() { 8, 7, 4 },
            new List<int>() { 4, 7, 8 },
            new List<int>() { 4, 1, 2 },
            new List<int>() { 2, 1, 4 },

            new List<int>() { 4, 7, 8, 9, 6 },
            new List<int>() { 4, 1, 2, 3, 6 },
            new List<int>() { 8, 7, 4, 1, 2 },
            new List<int>() { 2, 1, 4, 7, 8 },

            new List<int>() { 4, 1, 2, 3, 6, 9, 8, 7, 4 },
            new List<int>() { 4, 7, 8, 9, 6, 3, 2, 1, 4 },

            new List<int>() { 2, 1, 4, 7 },
            new List<int>() { 8, 7, 4, 1 },
    };
    }

    void FlipInput(List<int> defaultInput, out List<int> reversedInput)
    {
        reversedInput = new List<int>();

        for(int i = 0; i < ComboInputNormal.Count; i++)
        {
            if (InputCompare(defaultInput, ComboInputNormal[i]))
            {
                reversedInput = ComboInputReverse[i];
            }
        }
        
    }
}
