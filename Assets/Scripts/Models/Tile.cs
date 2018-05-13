using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

public enum  TileType {Empty, Grass, Dirt, Water, Stone, Sand, Snow};

public class Tile{

	TileType type = TileType.Empty;

	Action<Tile> cbTileTypeChanged;

	public bool walkable { get; protected set;}

	World world;

	public int x { get; private set; }
	public int y { get; private set; }
	public int z { get; private set; }

	public Tile (int x, int y, int z, World world){
		this.x = x;
		this.y = y;
		this.z = z;
		this.world = world;
	}

	public void SetTileType(Color c){
		
		if(CompareColors(c, world.colors[world.regions[0]])){
			this.type = TileType.Water;
			walkable = false;
			this.z = -4;

		}else if ( CompareColors(c, world.colors[world.regions[1]])){
			this.type = TileType.Water;
			walkable = false;
			this.z = -3;

		}else if (CompareColors(c, world.colors[world.regions[2]])){
			this.type = TileType.Water;
			walkable = false;
			this.z = -2;

		}else if (CompareColors(c, world.colors[world.regions[3]])){
			this.type = TileType.Water;
			walkable = false;
			this.z = -1;

		}else if (CompareColors(c, world.colors[world.regions[4]])){
			this.type = TileType.Sand;
			walkable = true;
			this.z = 0;

		}else if (CompareColors(c, world.colors[world.regions[5]])){
			this.type = TileType.Grass;
			walkable = true;
			this.z = 0;

		}else if (CompareColors(c, world.colors[world.regions[6]])){
			this.type = TileType.Grass;
			walkable = true;
			this.z = 1;

		}else if (CompareColors(c, world.colors[world.regions[7]])){
			this.type = TileType.Stone;
			walkable = false;
			this.z = 1;

		}else if (CompareColors(c, world.colors[world.regions[8]])){
			this.type = TileType.Stone;
			walkable = false;
			this.z = 2;

		}else if (CompareColors(c, world.colors[world.regions[9]])){
			this.type = TileType.Snow;
			walkable = false;
			this.z = 3;
		}
			
		//Debug.Log ("setting the tile type to " + type + "| Color = " + c);
	}
		
	public void setWalkable(bool w){
		this.walkable = w;
	}
	
	public void RegisterTileTypeChangedCallback(Action<Tile> callback){
		cbTileTypeChanged += callback;
	}
	public void UnregisterTileTypeChangedCallback(Action<Tile> callback){
		cbTileTypeChanged -= callback;
	}

	public TileType Type {
		get {
			return type;
		}
		set {
			type = value;
			//call the callback
			if(cbTileTypeChanged != null)
				cbTileTypeChanged(this);
		}
	}

	public void setZ (int z){
		this.z = z;
	}

	public Tile[] GetNeighbours(bool diagonals = false){
		Tile[] ns;

		if (diagonals == false) {
		// n,e,s,w
			ns = new Tile[4];
		} else {
		// n,e,s,w,ne,se,sw,nw
			ns = new Tile[8];
		}

		Tile n;
		n = world.GetTileAt (x, y+1);
		ns [0] = n;
		n = world.GetTileAt (x+1, y);
		ns [1] = n;
		n = world.GetTileAt (x, y-1);
		ns [2] = n;
		n = world.GetTileAt (x-1, y);
		ns [3] = n;

		if(diagonals == true){
			n = world.GetTileAt (x+1, y+1);
			ns [4] = n;
			n = world.GetTileAt (x+1, y-1);
			ns [5] = n;
			n = world.GetTileAt (x-1, y-1);
			ns [6] = n;
			n = world.GetTileAt (x-1, y+1);
			ns [7] = n;
		}

		return ns; 
	}

	public string toString(){
		return "tile_" + this.x + "_" + this.y;
	}



	public bool CompareColors(Color a, Color b){

		float diff_r = Mathf.Abs( a.r - b.r);
		float diff_g = Mathf.Abs( a.g - b.g);
		float diff_b = Mathf.Abs( a.b - b.b);

		if(diff_r < .01f && diff_g < .01f && diff_b < .01f)
			return true;
		return false;
	}
}
