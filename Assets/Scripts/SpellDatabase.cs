using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDatabase : MonoBehaviour {

    public Attack tripleFire, darkFire, dabThunder;

	// Use this for initialization
	void Start () {
        tripleFire = new Attack("Triple Fire", "Fire 3 shots", 10, 30, Attack.Element.Fire, Attack.ElementEffect.Burn, Attack.AttackType.MultipleBlast, 3,0.2f, 0, 2, 1, 0, "fireball");
        darkFire = new Attack("Dark Fire", "Fire a dark blast", 10, 15, Attack.Element.Fire, Attack.ElementEffect.Burst, Attack.AttackType.Blast, 4, 3, 4, 4, 0, "fireball");
        //dabThunder = new Attack("Dab Thunder", "Unleash Fearsome Bolts", 80, Attack.Element.Thunder, Attack.ElementEffect.Stun, Attack.AttackType.Special)
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
