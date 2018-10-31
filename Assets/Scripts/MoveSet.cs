using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSet : MonoBehaviour {

    SpellDatabase attackIndex;
    public Item.Spellbook spellBook_B_Button, spellBook_X_Button, spellBook_Y_Button, spellBook_R_Button, spellBook_L_Button;

    public Attack[] Attacks;

    
	
	void Start () {

        attackIndex = FindObjectOfType<SpellDatabase>();

        
        spellBook_B_Button = new Item.Spellbook();
        spellBook_X_Button = new Item.Spellbook();
        spellBook_Y_Button = new Item.Spellbook();
        spellBook_L_Button = new Item.Spellbook();
        spellBook_R_Button = new Item.Spellbook();

        
        
        spellBook_B_Button.attacks.Add(attackIndex.yeetFire);
        spellBook_B_Button.attacks.Add(attackIndex.tripleFire);
	}
	
	
	void Update () {
		
	}
}
