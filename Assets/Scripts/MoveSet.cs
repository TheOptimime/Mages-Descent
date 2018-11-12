using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSet : MonoBehaviour {

    public SpellDatabase attackIndex;
   public  List<List<Item.Spellbook>> spellBookLoadout;

    public List<Item.Spellbook> spellBookSet_A, spellBookSet_B, spellBookSet_C;
    public Item.Spellbook spellBook_B_Button, spellBook_X_Button, spellBook_Y_Button;

    public Attack[] Attacks;

    public int spellLoadOutSelected;

    private void Awake()
    {
        attackIndex = FindObjectOfType<SpellDatabase>();
    }

    void Start () {

        

        // B = 0, X = 1, Y = 2

        spellBook_B_Button = new Item.Spellbook();
        spellBook_X_Button = new Item.Spellbook();
        spellBook_Y_Button = new Item.Spellbook();

        spellBookSet_A = new List<Item.Spellbook>();
        spellBookSet_B = new List<Item.Spellbook>();
        spellBookSet_C = new List<Item.Spellbook>();

        spellBookLoadout = new List<List<Item.Spellbook>>();

        print(attackIndex);
        print(attackIndex.yeetFire);
        spellBook_B_Button.attacks.Add(attackIndex.yeetFire);
        print(spellBook_B_Button.attacks[0]);
        spellBook_B_Button.attacks.Add(attackIndex.tripleFire);

        spellBook_X_Button.attacks.Add(attackIndex.dabIce);

        spellBook_Y_Button.attacks.Add(attackIndex.darkFire);

        spellBookSet_A.Add(spellBook_B_Button);
        spellBookSet_A.Add(spellBook_X_Button);
        spellBookSet_A.Add(spellBook_Y_Button);

        spellBookSet_B = spellBookSet_C = spellBookSet_A;

        spellBookLoadout.Add(spellBookSet_A);
        spellBookLoadout.Add(spellBookSet_B);
        spellBookLoadout.Add(spellBookSet_C);
    }
	
	
	void Update () {

        print(attackIndex);
        
        print(attackIndex.AttackList[0]);
        print(attackIndex.yeetFire);
        print(spellBook_B_Button.attacks[0]);


        if(spellLoadOutSelected > 2)
        {
            spellLoadOutSelected = 0;
        }

        if(spellLoadOutSelected < 0)
        {
            spellLoadOutSelected = 2;
        }
		
	}
}
