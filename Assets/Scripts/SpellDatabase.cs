using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDatabase : MonoBehaviour {

    public List<Attack> AttackList;

    public Attack tripleFire, darkFire;

    List<int> quarterCircleDownRight, quarterCircleDownLeft, quarterCircleRightDown, quarterCircleLeftDown, quarterCircleUpRight, quarterCircleRightUp, quarterCircleUpLeft, quarterCircleLeftUp;
    List<int> halfCircleUnderLeftRight, halfCircleUnderRightLeft, halfCircleOverLeftRight, halfCircleRightLeft, halfCircleDownUpRight, halfCircleDownUpLeft, halfCircleUpDownRight, halfCircleUpDownLeft;
    List<int> fullCircleRightUp, fullCircleRightDown, fullCircleLeftUp, fullCircleLeftDown;
    List<int> fourCircleDownRight, fourCircleUpRight, fourCircleDownLeft, fourCircleUpLeft;

	
	void Awake () {
        InitializeJoyStickCommands();

        tripleFire = new Attack("TripleFire", "Blast 3 fireballs", quarterCircleDownRight, 5, new Vector2(200, 80), 5f, 40f, Attack.Element.Fire, Attack.ElementEffect.Burst, 0.8f, 1.6f, 1.3f, "fireball", 3, 0.8f, -3000.3f);
        darkFire = new Attack("Dark Fire", "Dark Fire", null, 1, new Vector2(0.2f, 0), 0.4f, 2.3f, Attack.Element.Fire, Attack.ElementEffect.None, Attack.AttackType.Blast, 3.5f, 0.4f, 0.34f, "fireball");

        
        //deLigma = new Attack("DeLigma", "Whats DeLigma?", 10, 5, Attack.Element.Blood, Attack.ElementEffect.Burst, Attack.AttackType.MultipleBlast, _ )

        AttackList = new List<Attack>();


        AttackList.Add(tripleFire);
        //AttackList[0].hitStun = 1.2f;

        AttackList.Add(darkFire);
        //AttackList[1].lifetime = 0.3f;
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
