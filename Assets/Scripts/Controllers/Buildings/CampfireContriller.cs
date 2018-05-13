using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CampfireContriller : MonoBehaviour {
	
	Campfire campfire;

	public int maxPop = 4;

	void OnEnable(){
		TimeManager.onHour += upgrade;

	}

	void OnDisable(){
		TimeManager.onHour -= upgrade;
	}

	// Use this for initialization
	void Start () {
		Tile t = WorldController.Instance.GetTileAt ((int)this.transform.position.x, (int)this.transform.position.y); 
		campfire = new Campfire (t, maxPop);
		WorldController.Instance.SetWalkable ((int)this.transform.position.x, (int)this.transform.position.y, 0);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void upgrade(){
		if(campfire.villagers.Count < maxPop){
			createNPC ();
		}
	}

	void createNPC(){
		Tile tile = WorldController.Instance.GetTileAt (campfire.tile.x + 1, campfire.tile.y);

		Vector2 pos = new Vector2 (campfire.tile.x + 1, campfire.tile.y);

		Villager v = NPCManager.Instance.createVillager (campfire,pos);
		v.MovementMode = MoveMode.Wander;

		v.SetHomeAndOffset (campfire, tile);
		campfire.AddVillager (v);
	}

}


