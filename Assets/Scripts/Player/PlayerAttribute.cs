using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fighter))]
public class PlayerAttribute : MonoBehaviour
{
    public Attack.Element startingSchool;
    public Affinity affinity;

    public Fighter fighter;

    void Start()
    {
        if(fighter == null)
        {
            fighter = GetComponent<Fighter>();
        }

        if (affinity == Affinity.Vengence)
        {
            fighter.health.maxHealth -= 20;
            fighter.rb.gravityScale = 6;
        }
        else if (affinity == Affinity.Guile)
        {
            fighter.maxNumberOfJumps = 3;
            fighter.rb.gravityScale = -2;
        }
        else if (affinity == Affinity.Remission)
        {
            fighter.health.maxHealth += 20;
            fighter.runSpeed -= fighter.runSpeed / 6;
        }

        if(startingSchool == Attack.Element.Fire)
        {
            fighter.moveset.spellBookSet_A.Add(new Item.Spellbook(Attack.Element.Fire));

            if (affinity == Affinity.Vengence)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
            else if (affinity == Affinity.Guile)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
            else if (affinity == Affinity.Remission)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
        }
        else if (startingSchool == Attack.Element.Ice)
        {
            fighter.moveset.spellBookSet_A.Add(new Item.Spellbook(Attack.Element.Ice));

            if (affinity == Affinity.Vengence)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
            else if (affinity == Affinity.Guile)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
            else if (affinity == Affinity.Remission)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
        }
        else if (startingSchool == Attack.Element.Thunder)
        {
            fighter.moveset.spellBookSet_A.Add(new Item.Spellbook(Attack.Element.Thunder));

            if (affinity == Affinity.Vengence)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
            else if (affinity == Affinity.Guile)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
            else if (affinity == Affinity.Remission)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
        }
        else if (startingSchool == Attack.Element.Arcane)
        {
            fighter.moveset.spellBookSet_A.Add(new Item.Spellbook(Attack.Element.Arcane));

            if (affinity == Affinity.Vengence)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
            else if (affinity == Affinity.Guile)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
            else if (affinity == Affinity.Remission)
            {
                //fighter.moveset.spellBookSet_A[0].attacks.Add();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
