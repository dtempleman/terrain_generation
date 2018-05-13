using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Movement : MonoBehaviour {
	float movementPercent;
	float speed = 2f;

	AStarPath ASPath;

	public bool DrawPathLine;



	public MoveMode mode = MoveMode.Idle;

	private float wanderDist = 0f;


	private float waiting;

	private List<Tile> path;


	void Start () {
		ASPath = new AStarPath (WorldController.Instance.graph);
		path = new List<Tile> ();
		//just for debugging
		//DrawPathLine = true;
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}
		
	private void Move(){ 
		waiting -= Time.deltaTime;

		if (DrawPathLine) {
			DrawPath ();
		}


		if(path.Count > 0){
			if (waiting < 0) {
				MoveThroughAStartPath ();
				waiting = .2f;
			}

		} else if(mode == MoveMode.Idle){
			if (waiting < 0) {
				
				Tile homeTile = this.GetComponent<VillagerController> ().villager.homeTile;
				if(homeTile.x != this.transform.position.x && homeTile.y != this.transform.position.y)
					AStarFindPathTo (homeTile);
				waiting = .5f;
			}
		}else if(mode == MoveMode.Wander){
			if (waiting < 0) {

				SetWanderDest ();
				waiting = .5f;
			}
		}else if(mode == MoveMode.Manual){
			
		}
	}

	public void SetToWanderMode(int dist){
		this.mode = MoveMode.Wander;
		this.wanderDist = dist;
	}

	public void SetIdleMode(){
		this.mode = MoveMode.Idle;
	}

	private void SetWanderDest(){
		Tile homeTile = this.GetComponent<VillagerController> ().villager.homeTile;
		if (homeTile.x >= 0 && homeTile.y >= 0) {
			

			int rx = (int)Random.Range (wanderDist * -1, wanderDist);
			int ry = (int)Random.Range (wanderDist * -1, wanderDist);
			Vector2 newDest = new Vector2 (homeTile.x + rx, homeTile.y + ry); 

			//if you cant walk on the destination tile, set a new one.
			while(WorldController.Instance.IsWalkable((int)newDest.x , (int)newDest.y) == 0){
				
				//Debug.Log (newDest + " not walkable");
				rx = (int)Random.Range (wanderDist*-1, wanderDist);
				ry = (int)Random.Range (wanderDist*-1, wanderDist);
				newDest = new Vector2 (homeTile.x + rx, homeTile.y + ry); 

			}
			//Debug.Log(newDest);
			AStarFindPathTo (WorldController.Instance.GetTileAt((int)newDest.x, (int)newDest.y));
		}
	}

	private void DrawPath(){
		
		if (path != null  && path.Count > 0) {
			Vector3 start = new Vector3 (this.transform.position.x + .5f, this.transform.position.y + .5f, -1);
			Vector3 end = new Vector3 (path [0].x + .5f, path [0].y + .5f, -1);
			Debug.DrawLine(start, end, Color.red);

			int i = 0;
			while (i < path.Count - 1) {
				start = new Vector3 (path [i].x + .5f, path [i].y + .5f, -1);
				end = new Vector3 (path [i+1].x + .5f, path [i+1].y + .5f, -1);
				Debug.DrawLine (start, end, Color.red); 
				i++;
			}
		}
	}


	private void MoveThroughAStartPath(){
		Tile Goal = path [0];
		//MoveToTile (Goal);
		this.transform.position = new Vector2(Goal.x, Goal.y);
		if(CurrentTile() == Goal){
			path.Remove (Goal); 
		}
	}

	void MoveToTile(Tile Goal){
		Tile currTile = CurrentTile ();

		float travel_dist = Mathf.Sqrt (Mathf.Pow (currTile.x - Goal.x, 2) + Mathf.Pow (currTile.y - Goal.y, 2));
		//Debug.Log ("travel_dist =" + travel_dist);

		float dist_this_frame = speed * Time.deltaTime;
		//Debug.Log ("dist_this_frame =" + dist_this_frame);

		float perc_this_frame = dist_this_frame / travel_dist;
		//Debug.Log ("perc_this_frame =" + perc_this_frame);

		movementPercent += perc_this_frame;

		float x = Mathf.Lerp (currTile.x, Goal.x, movementPercent);
		float y = Mathf.Lerp (currTile.y, Goal.y, movementPercent);




		if(movementPercent >= 1){
			movementPercent = 0;

		}

		this.transform.position = new Vector3 (x, y, 0f);
	}

	private void AStarFindPathTo(Tile goal){
		if (WorldController.Instance.IsWalkable (goal.x, goal.y) != 0) {
			path = ASPath.FindPath (CurrentTile (), goal);
		}
	}


	public Tile CurrentTile(){
		return WorldController.Instance.GetTileAt ((int)this.transform.position.x, (int)this.transform.position.y);
	}

	public void SetManualDest(int x, int y){
		ASPath = new AStarPath (WorldController.Instance.graph);
		AStarFindPathTo (WorldController.Instance.GetTileAt (x, y));
	}


}
