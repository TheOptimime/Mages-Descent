using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSet : MonoBehaviour {

    SpellDatabase attackIndex;

    public Attack[] Attacks;

	
	void Start () {
        attackIndex = FindObjectOfType<SpellDatabase>();
        Attacks = new Attack[3];
        Attacks[0] = attackIndex.tripleFire;
        Attacks[1] = attackIndex.darkFire;
	}
	
	
	void Update () {
		
	}
}
