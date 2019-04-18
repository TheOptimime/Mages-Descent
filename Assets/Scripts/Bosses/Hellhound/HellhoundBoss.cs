using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellhoundBoss : MonoBehaviour
{
    public HellhoundHead[] hellHoundHeads;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < hellHoundHeads.Length; i++)
        {
            if(hellHoundHeads[i].isDead == true && !hellHoundHeads[i].deathReported)
            {
                hellHoundHeads[i].gameObject.SetActive(false);

                if(hellHoundHeads[i+1] != null)
                {
                    hellHoundHeads[i + 1].gameObject.SetActive(true);
                }

                hellHoundHeads[i].deathReported = true;
            }
        }

    }
}
