using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Building{

	private int maxPopulation;

	public Campfire(Tile tile, int population){
		setTile (tile);
		maxPopulation = population;
		villagers = new List<Villager>();
	}

	public void AddVillager(Villager v){
		if (villagers.Count < maxPopulation) {
			villagers.Add (v);
		} else {
			Debug.Log ("You can't add more than " + maxPopulation + " villagers to this campfire.");
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
