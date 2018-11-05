using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

    public string name, description;

    public enum itemType
    {
        SpellBook,
        
    }

    public class Spellbook
    {

        public Attack.Element element;
        public List<Attack> attacks;

        int level, exp, SP;

        SpellDatabase spellDatabase;

        public Spellbook(Attack.Element element)
        {
            attacks = new List<Attack>();

        }

        public Spellbook()
        {
            attacks = new List<Attack>();

        }

        void AddAttack(Attack attack)
        {
            // check to see if another joystick command clashes
            for(int i = 0; i < attacks.Count; i++)
            {
                int matchCount = 0;

                if(attacks[i].joystickCommand.Count == attack.joystickCommand.Count)
                {
                    for (int joystickNumber = 0; joystickNumber < attacks[i].joystickCommand.Count; joystickNumber++)
                    {
                        if (attacks[i].joystickCommand[joystickNumber] == attack.joystickCommand[joystickNumber])
                        {
                            matchCount++;
                        }
                    }
                    if(matchCount == attacks[i].joystickCommand.Count)
                    {
                        attacks.Add(attack);
                        continue;
                    }
                }
                
            }
            
        }
    }
}
