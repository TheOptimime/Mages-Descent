using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDatabase : MonoBehaviour {

    Attack tripleFire;

	// Use this for initialization
	void Start () {
        tripleFire = new Attack(10, Attack.Element.Fire,Attack.AttackType.MultipleBlast, 3, 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
