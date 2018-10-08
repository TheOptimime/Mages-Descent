using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpellDatabase))]
public class GameManager : MonoBehaviour {

    int frameCount, totalFrameCount;
    float betterFrameCount;

    float gameTimer;

	// Use this for initialization
	void Start () {
        //QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        //Time.timeScale = 60;
	}
	
	// Update is called once per frame
	void Update () {
        totalFrameCount++;
        frameCount++;

        betterFrameCount += Time.deltaTime;

        if(totalFrameCount % 60 == 0) {
            frameCount = 0;
        }

        print("total frames " + totalFrameCount + " frame " + frameCount + " better: " + betterFrameCount);
        
        
        //print( "frame count: " + framecount + " Time: " + Time.time);
        //print("time: " + Time.time + " Delta Time: " + Time.deltaTime);
        
	}
}
