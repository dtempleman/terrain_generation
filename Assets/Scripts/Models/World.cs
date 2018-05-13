using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;


public class World{
	public float[] regions;
	public Dictionary<float, Color> colors;

	private int count;


	public Texture2D worldTexture{ get; protected set; }

	public int width { get; protected set; }
	public int height { get; protected set; }
	public Color[] worldColors{ get; protected set; }
	public int chunkSize { get; private set;}

	public Dictionary<Vector2, Chunk> chunkData { get; protected set;}

	public World(){
		FillHeightColors ();
	}

	public void SetTexture(Texture2D wt){
		worldTexture = wt;
		worldColors = worldTexture.GetPixels ();
		chunkData = new Dictionary<Vector2,Chunk> ();
		chunkSize = 10;
		this.width = wt.width;
		this.height = wt.height; 

	}

	public Chunk GetChunkAt(int x, int y){
		Vector2 cPos = new Vector2 (x, y); 
		//if it is not in memory
		if (chunkData.ContainsKey (cPos) == false) {
			Chunk chunk = new Chunk (cPos,this);

			chunkData.Add (cPos, chunk);
			count++;
			return chunk;

		} else {
			Chunk chunk = chunkData [cPos];
			return chunk;
		}
	}


	public Tile GetTileAt(int x, int y){

		int newX = (int)(x / chunkSize) * chunkSize;
		int newY = (int)(y / chunkSize) * chunkSize;

		int remX = x % chunkSize;
		int remY = y % chunkSize;

		Vector2 v = new Vector2 (newX, newY); 
		if(chunkData.ContainsKey(v)){
			Tile t = chunkData [v].getTileAt (remX, remY);

			return t; 
		}
		return null;
	}



	private void FillHeightColors(){
		regions =  new float[10]{.05f, .1f, .2f, .3f, .33f, .4f, .55f, .6f, .7f, 1f};
		colors = new Dictionary<float,Color> ();

		colors.Add (regions [0], new Color (0f, 0f, .7f));
		colors.Add (regions [1], new Color (0f, 0f, .8f));
		colors.Add (regions [2], new Color (0f, 0f, .9f));
		colors.Add (regions [3], new Color (0f, 0f, 1f));
		colors.Add (regions [4], new Color (1f, 1f, 0f));
		colors.Add (regions [5], new Color (0f, 1f, 0f));
		colors.Add (regions [6], new Color (0f, .9f, 0f));
		colors.Add (regions [7], new Color (.5f, .5f, .5f));
		colors.Add (regions [8], new Color (.7f, .7f, .7f));
		colors.Add (regions [9], new Color (1f, 1f, 1f));

	}

}
