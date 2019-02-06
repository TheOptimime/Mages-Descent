using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

    public string name, description;

    public enum ItemType
    {
        SpellBook,
        
    }

    public class Spellbook
    {

        public Attack.Element element;
        public List<Attack> attacks;

        int level, exp, expForNextLevel, SP;

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
            if(attack == null)
            {
                return;
            }

            if(attack.element != element)
            {
                Debug.Log("Spell does not match element");
                return;
            }
            

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
                        return;
                    }
                }
                
            }

            if(SP - attack.spellPoints < 0)
            {
                return;
            }

            SP -= attack.spellPoints;
            attacks.Add(attack);
            
        }

        void RemoveAttack(Attack attack)
        {
            if (attacks.Remove(attack)) {
                SP += attack.spellPoints;
            }
        }
        

        void LevelUp()
        {
            exp = expForNextLevel - exp;
            expForNextLevel += Mathf.RoundToInt(expForNextLevel * Random.Range(1.4f, 2.4f));
            if(exp < 0)
            {
                Debug.LogWarning("Something is Wrong With Levelling");
            }
        }
    }
}
