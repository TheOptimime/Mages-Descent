using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDatabase : MonoBehaviour {

    public List<Attack> AttackList;

    public Attack tripleFire, darkFire, dabThunder;

    List<int> quarterCircleDownRight, quarterCircleDownLeft, quarterCircleRightDown, quarterCircleLeftDown;
    List<int> halfCircleLeftRight, halfCircleRightLeft;

	// Use this for initialization
	void Start () {
        InitializeJoyStickCommands();

        tripleFire = new Attack("Triple Fire", "Fire 3 shots", 10, 30, Attack.Element.Fire, Attack.ElementEffect.Burn, Attack.AttackType.MultipleBlast, 3,0.2f, 0, 2, 1, 0, "fireball", quarterCircleDownRight);
        darkFire = new Attack("Dark Fire", "Fire a dark blast", 10, 15, Attack.Element.Fire, Attack.ElementEffect.Burst, Attack.AttackType.Blast, 4, 3, 4, 4, 0, "fireball");
        //dabThunder = new Attack("Dab Thunder", "Unleash Fearsome Bolts", 80, Attack.Element.Thunder, Attack.ElementEffect.Stun, Attack.AttackType.Special)
        AttackList = new List<Attack>();

        AttackList.Add(tripleFire);
        AttackList.Add(dabThunder);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeJoyStickCommands()
    {
        quarterCircleDownLeft = new List<int>();
        quarterCircleDownRight = new List<int>();
        quarterCircleLeftDown = new List<int>();
        quarterCircleRightDown = new List<int>();
        halfCircleLeftRight = new List<int>();
        halfCircleRightLeft = new List<int>();

        #region Down_Left
        quarterCircleDownLeft.Add(8);
        quarterCircleDownLeft.Add(7);
        quarterCircleDownLeft.Add(4);
        #endregion

        #region Left_Down
        quarterCircleLeftDown.Add(4);
        quarterCircleLeftDown.Add(7);
        quarterCircleLeftDown.Add(8);
        #endregion

        #region Down_Right
        quarterCircleDownRight.Add(8);
        quarterCircleDownRight.Add(9);
        quarterCircleDownRight.Add(6);
        #endregion

        #region Right_Down
        quarterCircleRightDown.Add(6);
        quarterCircleRightDown.Add(9);
        quarterCircleRightDown.Add(8);
        #endregion

        #region Left_Right
        halfCircleLeftRight.Add(4);
        halfCircleLeftRight.Add(7);
        halfCircleLeftRight.Add(8);
        halfCircleLeftRight.Add(9);
        halfCircleLeftRight.Add(6);
        #endregion

        #region Right_Left
        halfCircleRightLeft.Add(6);
        halfCircleRightLeft.Add(9);
        halfCircleRightLeft.Add(8);
        halfCircleRightLeft.Add(7);
        halfCircleRightLeft.Add(4);
        #endregion
    }
}
