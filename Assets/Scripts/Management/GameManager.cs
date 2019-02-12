using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpellDatabase))]
[RequireComponent(typeof(RespawnManager))]
public class GameManager : MonoBehaviour {

    int frameCount, totalFrameCount;
    float betterFrameCount;

    float gameTimer;

    GameObject[] players;
    RespawnManager rm;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        rm = GetComponent<RespawnManager>();

        int i = 1;

        foreach (GameObject player in players)
        {
            Fighter _player = player.GetComponent<Fighter>();

            if (_player != null)
            {
                // set references to respawn manager and stuff
                _player.PlayerID = i++;
            }
            else
            {
                // Player is AI
                PlayerAI _ai = player.GetComponent<PlayerAI>();
                _ai.PlayerID = i++;
            }
        }

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
	}
}
