using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputHandler : MonoBehaviour {

    bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    public GamePadState state;
    GamePadState prevState;
    public Fighter player;
    SpellDatabase spellDatabase;

    List<List<int>> ComboInputNormal, ComboInputReverse;

    public bool keyboardInputInUse, dPadInputInUse;

    [Range(0.2f, 0.8f)]
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
        print(player.gameObject.name);
        joystickRecord = new List<int>();
        InitializeJoyStickCommands();
    }

    void Update () {
        //int directionalInput;
        if(playerIndexSet != true)
        {
            FindController();
        }

        CheckForInputOverrides();

        HandleShoulderButtons();
        HandleButtons();
        HandleKeyboardAttacks();
        

        if (keyboardInputInUse)
        {
            HandleKeyboardDirection(out joystickPosition);
        }
        else if (dPadInputInUse)
        {
            HandleDPad(out joystickPosition);
        }
        else
        {
            HandleJoystick(out joystickPosition);
        }
        
            if (joystickRecord.Count == 0 && timeBeforeLastInput == 0 && joystickPosition != 5 )
            {
                joystickRecord.Add(joystickPosition);
                timeBeforeLastInput = Time.time;
                timeForNextInput = timeBeforeLastInput + 0.3f;
                //print("joystick input detected: " + joystickPosition);
            }
            else if (Time.time > timeForNextInput)
            {
                joystickRecord.Clear();
                timeBeforeLastInput = 0;
                //print("joystick record cleared");
            }
            else if (buttonPressed)
            {
                //print("Button Pressed, Input Cleared");
                joystickRecord.Clear();
                timeBeforeLastInput = 0;
            }
            else if(Time.time < timeForNextInput && joystickRecord.Count != 0 && joystickPosition != joystickRecord[joystickRecord.Count -1])
            {
                joystickRecord.Add(joystickPosition);
                timeBeforeLastInput = Time.time;
                timeForNextInput = timeBeforeLastInput + 0.3f;
                //print("joystick input detected");
            }

        //if (buttonPressed)
        {
//            print("state: " + state.ThumbSticks.Left.X);
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);

        FlushInputOverrides();
        buttonPressed = false;

        
    }

    private void FixedUpdate()
    {
        // Experimental Vibration Control
        
        GamePad.SetVibration(playerIndex, vibrateLeftMotor, vibrateRightMotor);
        
    }

    void HandleButtons()
    {
        List<int> frozenJoystickRecord = new List<int>();
        int buttonID;

        #region A Button
        if(prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
//            print("A Button Pressed");
            frozenJoystickRecord = joystickRecord;

            //print(quarterCircleDownRight);
            /*
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
            */
            player.jump = true;

        }
        else if(prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Pressed)
        {
            //print("A Button Held");
            
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
            buttonID = 0;
            // B button is pressed
            // Creates a record of the joystick inputs on button press
            frozenJoystickRecord = joystickRecord;
            buttonPressed = true;
            // Checking if the inputs entered matched any of the spells
            for (int i = 0; i < player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks.Count; i++)
            {
                int matchCount = 0;

                // Makes a temporary copy of this joystick command
                List<int> tempJoystickCommand = new List<int>();

                /*
                print(i);
                print(player.moveset.spellBookLoadout.Count);
                print(player.moveset.spellBookLoadout[0].Count);
                print(player.moveset.spellBookLoadout[0][0].attacks.Count);
                print(player.moveset.spellBookLoadout[0][0].attacks[0]);
                print(player.moveset.spellBookLoadout[0][0].attacks[1]);
                print(player.moveset.spellBookLoadout[0][0].attacks[0].name);
                print(player.moveset.spellBookLoadout[0][0].attacks[0].joystickCommand);
                */

                if (player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand != null)
                {
                    if (!player.isFacingRight)
                    {
                        FlipInput(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand, out tempJoystickCommand);
                    }
                    else
                    {
                        tempJoystickCommand = player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand;
                    }
                }

                if (tempJoystickCommand != new List<int>())
                {
                    if (tempJoystickCommand.Count == frozenJoystickRecord.Count)
                    {
                        for (int joystickNumber = 0; joystickNumber < tempJoystickCommand.Count; joystickNumber++)
                        {
                            //print(tempJoystickCommand[joystickNumber] + " : " + frozenJoystickRecord[joystickNumber]);

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
                        player.SetAttackQueue(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i]);
                        break;
                    }
                    
                }
                else if (player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand == null || i >= player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks.Count)
                {
                    // this is the generic attack
                    player.SetAttackQueue(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i]);
                    break;
                }

            }



        }
        else if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Pressed)
        {
            buttonID = 0;
            //print("B Button Held");
            if (player.attackIsInQueue)
            {
                player.RelayButtonInput();
            }
        }
        else if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released)
        {
            buttonID = 0;
            //print("B Button Released");
            if (player.attackIsInQueue)
            {
                player.OnAttackButtonRelease();
            }
            
        }
        #endregion

        #region X Button
        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
        {
            buttonID = 1;
            // B button is pressed
            // Creates a record of the joystick inputs on button press
            frozenJoystickRecord = joystickRecord;
            buttonPressed = true;
            // Checking if the inputs entered matched any of the spells
            for (int i = 0; i < player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks.Count; i++)
            {
                int matchCount = 0;

                // Makes a temporary copy of this joystick command
                List<int> tempJoystickCommand = new List<int>();


                if (player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand != null)
                {
                    if (!player.isFacingRight)
                    {
                        FlipInput(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand, out tempJoystickCommand);
                    }
                    else
                    {
                        tempJoystickCommand = player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand;
                    }
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
                        player.SetAttackQueue(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i]);
                        break;
                    }

                }
                else if (player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand == null || i >= player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks.Count)
                {
                    // this is the generic attack
                    player.SetAttackQueue(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i]);
                    break;
                }

            }



        }
        else if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Pressed)
        {
            buttonID = 1;
            //print("X Button Held");

            if (player.attackIsInQueue)
            {
                player.RelayButtonInput();
            }
            
        }
        else if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released)
        {
            buttonID = 1;
            //print("X Button Released");

            if (player.attackIsInQueue)
            {
                player.OnAttackButtonRelease();
            }
            
        }
        #endregion

        #region Y Button
        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            buttonID = 2;
            // B button is pressed
            // Creates a record of the joystick inputs on button press
            frozenJoystickRecord = joystickRecord;
            buttonPressed = true;

            // Checking if the inputs entered matched any of the spells
            for (int i = 0; i < player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks.Count; i++)
            {
                int matchCount = 0;

                // Makes a temporary copy of this joystick command
                List<int> tempJoystickCommand = new List<int>();

                
                if(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i] != null)
                {
                    print(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i]);

                    if (!player.isFacingRight)
                    {
                        FlipInput(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand, out tempJoystickCommand);
                    }
                    else
                    {
                        tempJoystickCommand = player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand;
                    }
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
                        player.SetAttackQueue(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i]);
                        break;
                    }

                }
                else if (player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i].joystickCommand == null || i >= player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks.Count)
                {
                    // this is the generic attack
                    player.SetAttackQueue(player.moveset.spellBookLoadout[player.moveset.spellLoadOutSelected][buttonID].attacks[i]);
                    break;
                }

            }



        }
        else if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Pressed)
        {
            buttonID = 2;
            //print("Y Button Held");

            if (player.attackIsInQueue)
            {
                player.RelayButtonInput();
            }
            
        }
        else if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released)
        {
            buttonID = 2;
            //print("Y Button Released");

            if (player.attackIsInQueue)
            {
                player.OnAttackButtonRelease();
            }
            
        }
        #endregion

        #region L Button
        if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            //print("L Button Pressed")
            player.moveset.spellLoadOutSelected++;
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
            player.moveset.spellLoadOutSelected--;
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
            player.isDashing = true;
        }
        else if (prevState.Triggers.Right >= 0.3f && state.Triggers.Right >= 0.3f)
        {
            print("R2 Button Held");
            
        }
        else if (prevState.Triggers.Right >= 0.3f && state.Triggers.Right <= 0.2f)
        {
            print("R2 Button Released");
            player.isDashing = false;
        }
        #endregion
    }

    void HandleDPad(out int directionalInput)
    {
        if (state.DPad.Up == ButtonState.Pressed || state.DPad.Down == ButtonState.Pressed)
        {
            dPadInputInUse = true;

            if (state.DPad.Up == ButtonState.Pressed)
            {
                #region DPad_Left
                if (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed)
                {
                    dPadInputInUse = true;
                    //print("Left Button pressed");
                    directionalInput = 1;
                    return;
                }
                else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Pressed)
                {
                    dPadInputInUse = true;
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
                dPadInputInUse = true;
                directionalInput = 4;
                print("Left Button pressed");
                return;
            }
            else if (prevState.DPad.Left == ButtonState.Pressed && state.DPad.Left == ButtonState.Pressed)
            {
                dPadInputInUse = true;
                print("Left Button Held");
                directionalInput = 4;
                return;
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
                return;
            }
            else if (prevState.DPad.Right == ButtonState.Pressed && state.DPad.Right == ButtonState.Pressed)
            {
                //print("Right Button Held");
                directionalInput = 6;
                return;
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
            dPadInputInUse = true;
            //print("Up Button pressed");
            directionalInput = 2;
            return;
        }
        else if (prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up == ButtonState.Pressed)
        {
            dPadInputInUse = true;
            //print("Up Button Held");
            directionalInput = 2;
            return;
        }
        else if (prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up == ButtonState.Released)
        {
            dPadInputInUse = true;
            //print("Up Button Released");
            directionalInput = 2;
            return;
        }
        #endregion

        #region DPad_Down
        if (prevState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed)
        {
            dPadInputInUse = true;
            //print("Down Button pressed");
            directionalInput = 8;
            return;
        }
        else if (prevState.DPad.Down == ButtonState.Pressed && state.DPad.Down == ButtonState.Pressed)
        {
            dPadInputInUse = true;
            //print("Down Button Held");
            directionalInput = 8;
            return;
        }
        else if (prevState.DPad.Down == ButtonState.Pressed && state.DPad.Down == ButtonState.Released)
        {
            dPadInputInUse = true;
            //print("Down Button Released");
            directionalInput = 8;
            return;
        }
        #endregion

        directionalInput = 5;
    }

    void HandleKeyboardDirection(out int directionalInput)
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            // Either Up or down is held

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    // Pressed Left
                    directionalInput = 1;
                    return;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    // Pressed Right
                    directionalInput = 3;
                    return;
                }
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    // Pressed Left
                    directionalInput = 7;
                    return;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    // Pressed Right
                    directionalInput = 9;
                    return;
                }
            }


            else
            {
                if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    directionalInput = 2;
                    return;
                }
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    directionalInput = 8;
                    return;
                }

            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                // Pressed Left
                directionalInput = 4;
                return;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                // Pressed Right
                directionalInput = 6;
                return;
            }
        }

        // return default value
        directionalInput = 5;

    }

    void HandleKeyboardAttacks()
    {

    }

    void HandleJoystick(out int directionalInput)
    {
        // copies joystick values
        joystick = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        Vector2 joystickRaw = Vector2.zero;

        // Applies Deadzone and gives raw values
        joystickRaw.x = joystick.x > deadzone || joystick.x < -deadzone ? Mathf.Sign(joystick.x) : 0;
        joystickRaw.y = joystick.y > deadzone || joystick.y < -deadzone ? Mathf.Sign(joystick.y) : 0;

        // Math stuff to find which position the joystick is in
        directionalInput = (int)((joystickRaw.x + 2) + (joystickRaw.y == 1 ? -joystickRaw.y + 1 : joystickRaw.y == -1 ? joystickRaw.y + 7 : 3));
    }

    void CheckForInputOverrides()
    {
        // Checks for input from keyboard and dpad then overrides in that order if found
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
        // Disables keyboard and dpad override
        dPadInputInUse = false;
        keyboardInputInUse = false;
    }

    void FindController()
    {
        // Temporary, At this stage we may want to set this manually... but we won't

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
            if (!Equals(a[i], b[i]))
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
