using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn : Building {
	public Villager InnKeeper { get; protected set; } 
	public Vector2 InnKeeperPosition = new Vector2(4,6);
	private int MaxGuests;


	public Inn(Tile tile, int guests){
		setTile (tile);
		villagers = new List<Villager> ();
		MaxGuests = guests;
	}
		
	public Villager AddInnKeeper(Villager v){
		Villager prev = this.InnKeeper;
		this.InnKeeper = v;
		return prev; 
	}

	public void AddGuest(Villager v){
		if (villagers.Count < MaxGuests) {
			villagers.Add (v);
		} else {
			Debug.Log ("You can't add more than " + MaxGuests + " Guests to this Inn.");
		}
	}

	public override Villager RemoveVillager(Villager v){
		if (villagers != null && villagers.Count > 0 && villagers.Contains(v)) {
			villagers.Remove (v);
			return v;
		} else {
			Debug.Log ("there are no villagers to be taken from the campfire.");
			return null;
		}
	}
}
