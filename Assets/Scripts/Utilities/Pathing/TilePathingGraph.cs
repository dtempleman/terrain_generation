using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathingGraph{
	public Dictionary<Tile, PathNode<Tile>> nodes;
	List<Chunk> ChunksGraphed;
	World world;

	public TilePathingGraph(World world, bool full = false){
		this.world = world;
		ChunksGraphed = new List<Chunk> ();
		nodes = new Dictionary<Tile, PathNode<Tile>> ();
		if (full) {
			float start = Time.realtimeSinceStartup;
			CreateFullGraph (); 
			float end = Time.realtimeSinceStartup;
			Debug.Log ("Created full new graph of nodes in " + (end - start) + " seconds.");
		} else {
			float start = Time.realtimeSinceStartup;
			CreateGraph (); 
			float end = Time.realtimeSinceStartup;
			Debug.Log ("Created new graph of nodes in " + (end - start) + " seconds.");
		}
	}

	private void CreateGraph(){
		
		foreach (Vector2 v in world.chunkData.Keys) {
			Chunk c = world.chunkData[v];
			AddChunkToGraph (c);
		}
		UpdateEdges ();
	}

	public void UpdateGraph(){
		float start = Time.realtimeSinceStartup;

		bool updated = false;

		//adds all walkable nodes in all the loaded chunks.
		foreach (Vector2 v in world.chunkData.Keys) {

			Chunk c = world.chunkData[v];
			if (ChunksGraphed.Contains (c) == false) {
				updated = true;
				AddChunkToGraph (c);
			}
		}

		if (updated) {
			UpdateEdges ();
			float end = Time.realtimeSinceStartup;
			//Debug.Log ("Updated graph in " + (end - start) + " seconds.");
		}
	}



	private void AddChunkToGraph(Chunk c){

		for (int x = 0; x < c.size; x++) {
			for (int y = 0; y < c.size; y++) {
				Tile t = c.getTileAt (x, y);
				if (t.walkable) {
					PathNode<Tile> n = new PathNode<Tile> ();
					n.data = t;
					nodes.Add (t, n);
				}
			}
		}
		ChunksGraphed.Add (c);
	}


	private void UpdateEdges(){
		
		foreach(Tile t in nodes.Keys){
			PathNode<Tile> n = nodes [t];
			Tile[] neighbours = t.GetNeighbours (true);

			for (int i = 0; i < neighbours.Length; i++) {
				if (neighbours [i] != null && neighbours [i].walkable == true) {
					if (nodes.ContainsKey (neighbours [i])) {

						if (n.edges == null)
							n.edges = new PathEdge<Tile>[8];

						for (int j = 0; j < n.edges.Length; j++) {
							if (n.edges [j] == null) {
								PathEdge<Tile> e = new PathEdge<Tile> ();
								e.cost = 1;
								e.node = nodes [neighbours [i]];
								n.edges [i] = e;

							}
						}	
					}
				}
			}
		}
	}

	public PathNode<Tile> GetNode(Tile t){	
		if(nodes.ContainsKey(t))
			return nodes [t]; 
		return null;
	}
		
	public void CreateFullGraph(){

	}
		
}
