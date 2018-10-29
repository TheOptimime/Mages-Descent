using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDatabase : MonoBehaviour {

    public List<Attack> AttackList;

    public Attack tripleFire, darkFire, dabThunder, yeetFire;

    List<int> quarterCircleDownRight, quarterCircleDownLeft, quarterCircleRightDown, quarterCircleLeftDown;
    List<int> halfCircleLeftRight, halfCircleRightLeft;

	// Use this for initialization
	void Start () {
        InitializeJoyStickCommands();

        tripleFire = new Attack("Triple Fire", "Fire 3 shots", 10, 30, Attack.Element.Fire, Attack.ElementEffect.Burn, Attack.AttackType.MultipleBlast, 3,0.2f, 0, 2, 1, 0, "fireball", quarterCircleDownRight);
        darkFire = new Attack("Dark Fire", "Fire a dark blast", 10, 15, Attack.Element.Fire, Attack.ElementEffect.Burst, Attack.AttackType.Blast, 4, 3, 4, 4, 0, "fireball");
        //dabThunder = new Attack("Dab Thunder", "Unleash Fearsome Bolts", 80, Attack.Element.Thunder, Attack.ElementEffect.Stun, Attack.AttackType.Special)

        yeetFire = new Attack("Yeeet Fire", "Fire a dank blast", 10, 20, Attack.Element.Fire, Attack.ElementEffect.Burst, Attack.AttackType.Blast, 4, 3, 4, 4, 0, 0, "fireball",halfCircleLeftRight);

        AttackList = new List<Attack>();


        AttackList.Add(tripleFire);
        //AttackList[0].chargeTime = 5;

        AttackList.Add(darkFire);
        AttackList.Add(yeetFire);
    }
	

    void InitializeJoyStickCommands()
    {
        quarterCircleDownLeft = new List<int>() {8,7,4};
        quarterCircleDownRight = new List<int>() {8,9,6};
        quarterCircleLeftDown = new List<int>() {4,7,8};
        quarterCircleRightDown = new List<int>() {6,9,8};
        halfCircleLeftRight = new List<int>() {4,7,8,9,6};
        halfCircleRightLeft = new List<int>() {6,9,8,7,4};
    }
}
