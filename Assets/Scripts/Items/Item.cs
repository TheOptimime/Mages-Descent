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

        public Spellbook(Attack.Element _element)
        {
            attacks = new List<Attack>();
            element = _element;
        }

        public Spellbook()
        {
            attacks = new List<Attack>();
            Debug.Log("spellbook has no element");
        }

        void AddAttack(Attack attack)
        {
            // check to see if another joystick command clashes
            for(int i = 0; i < attacks.Count; i++)
            {
                int matchCount = 0;

                if(attacks[i]._joystickCommand.Count == attack._joystickCommand.Count)
                {
                    for (int joystickNumber = 0; joystickNumber < attacks[i]._joystickCommand.Count; joystickNumber++)
                    {
                        if (attacks[i]._joystickCommand[joystickNumber] == attack._joystickCommand[joystickNumber])
                        {
                            matchCount++;
                        }
                    }
                    if(matchCount == attacks[i]._joystickCommand.Count)
                    {
                        attacks.Add(attack);
                        continue;
                    }
                }
                
            }
            
        }
    }
}
