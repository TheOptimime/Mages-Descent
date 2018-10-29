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
    }
}
