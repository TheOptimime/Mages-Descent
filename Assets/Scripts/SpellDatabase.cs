using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDatabase : MonoBehaviour {

    public List<Attack> AttackList;

    public Attack tripleFire, darkFire, dabThunder, yeetFire;

    List<int> quarterCircleDownRight, quarterCircleDownLeft, quarterCircleRightDown, quarterCircleLeftDown, quarterCircleUpRight, quarterCircleRightUp, quarterCircleUpLeft, quarterCircleLeftUp;
    List<int> halfCircleUnderLeftRight, halfCircleUnderRightLeft, halfCircleOverLeftRight, halfCircleRightLeft, halfCircleDownUpRight, halfCircleDownUpLeft, halfCircleUpDownRight, halfCircleUpDownLeft;
    List<int> fullCircleRightUp, fullCircleRightDown, fullCircleLeftUp, fullCircleLeftDown;
    List<int> fourCircleDownRight, fourCircleUpRight, fourCircleDownLeft, fourCircleUpLeft;

	// Use this for initialization
	void Start () {
        InitializeJoyStickCommands();

        tripleFire = new Attack("Triple Fire", "Fire 3 shots", 10, 30, Attack.Element.Fire, Attack.ElementEffect.Burn, Attack.AttackType.MultipleBlast, 3,0.2f, 0, 2, 1, 0, "fireball", quarterCircleDownRight);
        darkFire = new Attack("Dark Fire", "Fire a dark blast", 10, 15, Attack.Element.Fire, Attack.ElementEffect.Burst, Attack.AttackType.Blast, 4, 3, 4, 4, 0, "fireball");
        //dabThunder = new Attack("Dab Thunder", "Unleash Fearsome Bolts", 80, Attack.Element.Thunder, Attack.ElementEffect.Stun, Attack.AttackType.Special)

        yeetFire = new Attack("Yeeet Fire", "Fire a dank blast", 10, 20, Attack.Element.Fire, Attack.ElementEffect.Burst, Attack.AttackType.Blast, 4, 3, 4, 4, 0, 0, "fireball",halfCircleUnderLeftRight);

        AttackList = new List<Attack>();


        AttackList.Add(tripleFire);
        //AttackList[0].chargeTime = 5;

        AttackList.Add(darkFire);
        AttackList.Add(yeetFire);
    }
	

    void InitializeJoyStickCommands()
    {
        #region Right Side
        quarterCircleDownRight = new List<int>() { 8, 9, 6 };
        quarterCircleRightDown = new List<int>() { 6, 9, 8 };
        quarterCircleRightUp = new List<int>() { 6, 3, 2 };
        quarterCircleUpRight = new List<int>() { 2, 3, 6 };

        halfCircleUnderRightLeft = new List<int>() { 6, 9, 8, 7, 4 };
        halfCircleOverLeftRight = new List<int>() { 4, 1, 2, 3, 6 };
        halfCircleDownUpRight = new List<int>() { 8, 9, 6, 3, 2 };
        halfCircleUpDownRight = new List<int>() { 2, 3, 6, 9, 8 };

        fullCircleRightUp = new List<int>() { 6, 3, 2, 1, 4, 7, 8, 9, 6};
        fullCircleRightDown = new List<int>() { 6, 9, 8, 7, 4, 1, 2, 3, 6 };

        fourCircleUpRight = new List<int>() { 2, 3, 6, 9 };
        fourCircleDownRight = new List<int>() { 8, 9, 6, 3 };
        #endregion

        #region Left Side
        quarterCircleDownLeft = new List<int>() { 8, 7, 4 };
        quarterCircleLeftDown = new List<int>() { 4, 7, 8 };
        quarterCircleLeftUp = new List<int>() { 4, 1, 2 };
        quarterCircleUpLeft = new List<int>() { 2, 1, 4 };

        halfCircleUnderLeftRight = new List<int>() { 4, 7, 8, 9, 6 };
        halfCircleOverLeftRight = new List<int>() { 4, 1, 2, 3, 6 };
        halfCircleDownUpLeft = new List<int>() { 8, 7, 4, 1, 2 };
        halfCircleUpDownLeft = new List<int>() { 2, 1, 4, 7, 8 };

        fullCircleLeftUp = new List<int>() { 4, 1, 2, 3, 6, 9, 8, 7, 4 };
        fullCircleLeftDown = new List<int>() { 4, 7, 8, 9, 6, 3, 2, 1, 4 };

        fourCircleDownLeft = new List<int>() { 8, 7, 4, 1 };
        fourCircleUpLeft = new List<int>() { 2, 1, 4, 7 };
        #endregion
  
    }
}
