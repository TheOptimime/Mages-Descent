using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSet : MonoBehaviour {

    SpellDatabase attackIndex;

    public Attack[] Attacks;

	// Use this for initialization
	void Start () {
        attackIndex = FindObjectOfType<SpellDatabase>();
        Attacks = new Attack[3];
        Attacks[0] = attackIndex.tripleFire;
        Attacks[1] = attackIndex.darkFire;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
