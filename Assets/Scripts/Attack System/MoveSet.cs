using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSet : MonoBehaviour {

    public SpellDatabase attackIndex;
    public List<List<Item.Spellbook>> spellBookLoadout;

    public List<Item.Spellbook> spellBookSet_A, spellBookSet_B, spellBookSet_C;
    public Item.Spellbook spellBook_B_Button, spellBook_X_Button, spellBook_Y_Button;

    //public Attack[] Attacks;

    public int spellLoadOutSelected;

    private void Awake()
    {
        attackIndex = FindObjectOfType<SpellDatabase>();
    }

    void Start() {

        // Initialize the 3 spellbook buttons
        spellBook_B_Button = new Item.Spellbook();
        spellBook_X_Button = new Item.Spellbook();
        spellBook_Y_Button = new Item.Spellbook();

        // Initialize the Spellbook Sets
        spellBookSet_A = new List<Item.Spellbook>();
        spellBookSet_B = new List<Item.Spellbook>();
        spellBookSet_C = new List<Item.Spellbook>();

        // Initialize the Spellbook Loadout
        spellBookLoadout = new List<List<Item.Spellbook>>();


        // Adds The Spellbooks to the Spellbook set
        for (int i = 0; i <= 2; i++)
        {
            spellBookSet_A.Add(new Item.Spellbook((Attack.Element)Random.Range(1, 6)));
            spellBookSet_B.Add(new Item.Spellbook((Attack.Element)Random.Range(1, 6)));
            spellBookSet_C.Add(new Item.Spellbook((Attack.Element)Random.Range(1, 6)));
            

            foreach(Attack a in attackIndex.AttackList)
            {
                if(spellBookSet_A[i].element == a.element)
                {
                    spellBookSet_A[i].attacks.Add(a);

                }

                if (spellBookSet_B[i].element == a.element)
                {
                    spellBookSet_B[i].attacks.Add(a);
                }

                if (spellBookSet_C[i].element == a.element)
                {
                    spellBookSet_C[i].attacks.Add(a);
                }

            }

            //print(spellBookSet_A[i].element + " " + spellBookSet_A[i].attacks.Count);
            //print(spellBookSet_B[i].element + " " + spellBookSet_B[i].attacks.Count);
            //print(spellBookSet_C[i].element + " " + spellBookSet_C[i].attacks.Count);
        }

        
        // Adds Spellbooks to loadout
        spellBookLoadout.Add(spellBookSet_A);
        spellBookLoadout.Add(spellBookSet_B);
        spellBookLoadout.Add(spellBookSet_C);
    }
	
	
	void Update () {

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
