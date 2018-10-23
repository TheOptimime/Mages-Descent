using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    List<Item> playerItems;
    public ItemDatabase itemDB;

	// Use this for initialization
	void Start () {
        playerItems = new List<Item>();
        itemDB = FindObjectOfType<ItemDatabase>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
