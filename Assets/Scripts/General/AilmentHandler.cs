using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilmentHandler : MonoBehaviour {

    Health health;

    float poisonDamage, poisonTimer, poisonRate, poisonEndTime;
    bool poisonRateSet;

    public enum Ailments
    {
        None,
        Poisoned,
        Burned,
        Bleeding,
        Stunned,
        Frozen
    }

    public Ailments ailment;

	
	void Start () {
		
	}
	
	
	void Update () {
		
        switch (ailment)
        {
            case Ailments.None:
                break;

            case Ailments.Poisoned:
                if (poisonRateSet != true)
                {
                    poisonTimer = Time.deltaTime + poisonRate;
                    poisonRateSet = true;
                    poisonEndTime = (Time.deltaTime * Random.Range(1.4f, 2.4f));
                }

                if (poisonRateSet)
                {
                    if(Time.deltaTime >= poisonTimer)
                    {
                        health.currentHealth -= poisonDamage+=(poisonDamage/2);
                        poisonTimer = Time.deltaTime + poisonRate;
                        poisonRate -= Random.Range(0.2f, 0.4f);

                        if(poisonRate <= 0 || poisonEndTime < poisonTimer)
                        {
                            ailment = Ailments.None;
                        }
                    }
                }

                break;

            case Ailments.Burned:
                break;

            case Ailments.Bleeding:
                break;

            case Ailments.Stunned:
                break;

            case Ailments.Frozen:
                break;
        }

        if(ailment != Ailments.Poisoned)
        {
            poisonRateSet = false;
        }
	}
}
