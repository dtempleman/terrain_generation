using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

public class Chunk{
	
	public GameObject go { get; private set;}

	public Vector2 chunkPos { get; protected set;}
	public Tile[,] tiles { get; protected set;}
	public int size { get; private set; }

	public World world { get; protected set; }

	public Chunk(Vector2 pos, World world){
		this.world = world;
		chunkPos = pos;
		size = world.chunkSize;
		int h = world.width; 

		tiles = new Tile[size, size];

		for (int i = (int) pos.x; i < (int) pos.x + size; i++) {
			for (int j = (int) pos.y; j < (int) pos.y + size; j++) {
				int x = i - (int) pos.x;
				int y = j - (int) pos.y;
			
				tiles [x,y] = new Tile(i, j, 0, world);

				tiles[x,y].SetTileType(world.worldColors[i+j*h]);


			}
		}
	}
		
	//load form save
	public Chunk(Vector2 pos){

	}

	public Tile getTileAt(int x, int y){
		return tiles [x, y]; 
	}


}
