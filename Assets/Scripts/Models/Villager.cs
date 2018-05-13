using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  MoveMode {Idle, Wander, Manual};

public class Villager {
	public Building home { get; protected set;}
	public Tile homeTile { get; protected set;}

	public MoveMode MovementMode = MoveMode.Idle;

	static int nextId = 0;

	public string name { get; private set; }
	public int id { get; protected set; }

	public Villager(Building home){
		this.home = home;
		this.id = nextId;
		nextId++;
		this.name = id.ToString ();
	}

	public void SetHomeAndOffset(Building home, Tile offset){
		this.home = home; 

		this.homeTile = offset;
	}

}
