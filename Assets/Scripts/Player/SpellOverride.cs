using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellOverride : MonoBehaviour
{
    public Attack[] SpellSetA, SpellSetB, SpellSetC;
    Item.Spellbook SpellBook_A, SpellBook_B, SpellBook_C;
    MoveSet moveset;
    
    void Start()
    {
        moveset = GetComponent<MoveSet>();
        moveset.spellBookSet_A = new List<Item.Spellbook>();
        moveset.spellBookSet_B = new List<Item.Spellbook>();
        moveset.spellBookSet_C = new List<Item.Spellbook>();

        moveset.spellBookSet_A.Add(new Item.Spellbook());
        moveset.spellBookSet_B.Add(new Item.Spellbook());
        moveset.spellBookSet_C.Add(new Item.Spellbook());

        foreach(Attack atk in SpellSetA)
        {
            SpellBook_A.attacks.Add(atk);
        }
        foreach(Attack atk in SpellSetB)
        {
            SpellBook_B.attacks.Add(atk);
        }
        foreach(Attack atk in SpellSetC)
        {
            SpellBook_C.attacks.Add(atk);
        }

        moveset.spellBookSet_A.Add(SpellBook_A);
        moveset.spellBookSet_A.Add(SpellBook_B);
        moveset.spellBookSet_A.Add(SpellBook_C);

        moveset.spellBookSet_C = moveset.spellBookSet_B = moveset.spellBookSet_A;

        //moveset.spellBook_B_Button;
    }

    
    void Update()
    {
        
    }
}
