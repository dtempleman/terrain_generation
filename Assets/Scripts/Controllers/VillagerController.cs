using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour {
	public Vector2 homeTile { get; protected set; }
	private Movement moveController;

	public Villager villager { get; protected set;}

	public string NPCName;


	public void AddVillagerData(Villager data){
		this.villager = data;
		this.NPCName = villager.name;
	}

	public void AddHomeTile(Vector3 newHome){
		this.homeTile = newHome;
	}

	// Use this for initialization
	void Start () {
		moveController = GetComponent<Movement> ();
		//Debug.Log ("Villager: " + villager.id + ". Name: " + villager.name + ". home: " + villager.home + " at " + villager.home.tile.ToString()); 
	}

	// Update is called once per frame
	void Update () {
		if (villager.MovementMode == MoveMode.Wander) {
			moveController.SetToWanderMode (5);
		} else if(villager.MovementMode == MoveMode.Idle){
			moveController.SetToWanderMode (2);
		}

	}

}
