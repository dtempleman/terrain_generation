using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class AStarPath{

	TilePathingGraph graph;

	public AStarPath(TilePathingGraph graph){
		this.graph = graph;
	}

	public List<Tile> FindPath(Tile tileStart, Tile tileGoal){
	
		PathNode<Tile> start = graph.nodes [tileStart];

		bool good = false;
		foreach (PathEdge<Tile> e in start.edges) {
			if (e != null) {
				if (e.node != null)
					good = true;
			}
		}
		if (!good) {
			Debug.Log ("start has no out edges");
			return null;
		}
			
		PathNode<Tile> goal = graph.nodes [tileGoal];


		List<PathNode<Tile>> closedSet = new List<PathNode<Tile>> ();

		SimplePriorityQueue<PathNode<Tile>> openSet = new SimplePriorityQueue<PathNode<Tile>>(); 

		if (graph.GetNode (tileStart) == null)
			Debug.LogError ("AStarPath: the starting node is not in the graph");
		if (graph.GetNode (tileGoal) == null)
			Debug.LogError ("AStarPath: the ending node is not in the graph");

		openSet.Enqueue (start,0f);

		Dictionary<PathNode<Tile>, PathNode<Tile>> cameFrom = new Dictionary<PathNode<Tile>, PathNode<Tile>> ();

		Dictionary<PathNode<Tile>, float> gScore = new Dictionary<PathNode<Tile>, float> ();
		foreach(PathNode<Tile> n in graph.nodes.Values){
			gScore [n] = Mathf.Infinity;
		}
		gScore [start] = 0f;

		Dictionary<PathNode<Tile>, float> fScore = new Dictionary<PathNode<Tile>, float> ();
		foreach(PathNode<Tile> n in graph.nodes.Values){
			fScore [n] = Mathf.Infinity;
		}
		fScore [start] = CostEstimate(start, goal);

		while (openSet.Count > 0) {
			PathNode<Tile> current = openSet.Dequeue ();

			if (current == goal) {
				return ReconstructPath (cameFrom, current);;
			}

			closedSet.Add (current);

			foreach (PathEdge<Tile> neighbour in current.edges) {
				if (neighbour == null) {
					continue;
				}

				if (closedSet.Contains (neighbour.node)) {
					continue;
				}
				if(WorldController.Instance.IsWalkable(neighbour.node.data.x, neighbour.node.data.y) == 0){
					continue;
				}
				float temp_gScore = gScore [current] + DistBetweenNodes (current, neighbour.node);

				if (openSet.Contains (neighbour.node) && temp_gScore >= gScore [neighbour.node]) {
					continue;
				}

				cameFrom [neighbour.node] = current;
				gScore [neighbour.node] = temp_gScore;
				fScore [neighbour.node] = (gScore [neighbour.node] + CostEstimate (neighbour.node, goal));

				if (openSet.Contains (neighbour.node) == false) {
					openSet.Enqueue (neighbour.node, fScore [neighbour.node]);
				}
			}
		}
		Debug.Log ("there is no path to the goal "+ tileGoal.toString() );
		return null; 

	}

	float CostEstimate(PathNode<Tile> a, PathNode<Tile> b){
		return Mathf.Sqrt (
					Mathf.Pow (a.data.x - b.data.x, 2) +
					Mathf.Pow (a.data.y - b.data.y, 2)
					);
	}

	float DistBetweenNodes(PathNode<Tile> a, PathNode<Tile> b){
		if (Mathf.Abs (a.data.x - b.data.x) + Mathf.Abs (a.data.y - b.data.y) == 1)
			return 1f;
		if(Mathf.Abs (a.data.x - b.data.x) == 1 && Mathf.Abs (a.data.y - b.data.y) == 1)
			return 1.1f;
		
		return Mathf.Sqrt (
			Mathf.Pow (a.data.x - b.data.x, 2) +
			Mathf.Pow (a.data.y - b.data.y, 2)
		);
	}

	List<Tile> ReconstructPath(Dictionary<PathNode<Tile>, PathNode<Tile>> cameFrom, PathNode<Tile> current){
		 List <Tile> path = new List<Tile> ();

		path.Add (current.data);
		while (cameFrom.ContainsKey (current)) {
			current = cameFrom [current];
			path.Add (current.data);
		}
		path.Reverse ();
		Tile t = path[0];
		path.Remove(t); 
		return path;
	}

}
