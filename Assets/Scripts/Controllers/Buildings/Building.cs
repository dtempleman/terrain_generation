using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

	public List<Villager> villagers { get; protected set; }

	public Tile tile { get; protected set; }

	public Building(){
	
	}

	public void setTile(Tile tile){
		this.tile = tile;
	}

	public virtual Villager RemoveVillager (Villager v){
		return null;
	}
}
