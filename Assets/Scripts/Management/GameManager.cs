using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpellDatabase))]
[RequireComponent(typeof(RespawnManager))]
public class GameManager : MonoBehaviour {

    public static GameManager instance;

    int frameCount, totalFrameCount;
    float betterFrameCount;

    float gameTimer;
    
    GameObject[] players;
    RespawnManager rm;

    public GameObject mainPlayer;
    public GameObject mainPlayerCamera;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {

        if(instance == null)
        {
            instance = this;
        }

        if(instance != this)
        {
            Destroy(this.gameObject);
        }

        mainPlayer = FindObjectOfType<Fighter>().gameObject;
        if(mainPlayerCamera == null)
        mainPlayerCamera = FindObjectOfType<Camera>().gameObject;

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

    private void OnLevelWasLoaded(int level)
    {
        BGSwap();
        mainPlayer.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    void BGSwap()
    {
        foreach(Transform child in mainPlayerCamera.transform)
        {
            Destroy(child.gameObject);
        }

        FindObjectOfType<ParallaxBackground>().parallaxCamera = mainPlayerCamera.GetComponent<ParallaxCamera>();

        GameObject BG;

        if (BG = GameObject.Find("BG_Stuff"))
        {
            print("BG Switch Type 1");
            BG.transform.parent = mainPlayerCamera.transform;
        }

        if(BG != null)
        {
            print("not null");
            BG.transform.parent = mainPlayerCamera.transform;
            BG.transform.position = mainPlayer.transform.position;
        }
        
    }
}
