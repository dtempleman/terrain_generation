using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnController : MonoBehaviour {
	
	Inn inn;
	public int MaximumGuests;


	public GameObject wall; 
	public GameObject floor;
	public GameObject stairs;
	public GameObject counter;

	private Color32 wallColor;
	private Color32 floorColor;
	private Color32 stairsColor;
	private Color32 counterColor;


	public Texture2D texture;
	public Color[] colors;

	void OnEnable(){
		TimeManager.onHour += FindInnKeeper;
		TimeManager.onDay += NewGuests;
	}

	void OnDisable(){
		TimeManager.onHour -= FindInnKeeper;
		TimeManager.onDay -= NewGuests;
	}

	void Start () {
		Tile t = WorldController.Instance.GetTileAt ((int)this.transform.position.x, (int)this.transform.position.y); 
		inn = new Inn (t, 6);
		GenerateGameObjects ();
	}


	void Update () {
		
	}

	private void FindInnKeeper(){
		if(inn.InnKeeper == null){
			Villager v = NPCManager.Instance.FindAvailableWorker (inn,inn.InnKeeperPosition);
			if(v != null){
				v.MovementMode = MoveMode.Idle;
				inn.AddInnKeeper (v);
			}
		}
	}

	public void NewGuests(){
		RemoveGuests ();
		if(inn.InnKeeper != null ){
			for (int i = 0; i < MaximumGuests; i++) {
				createNPC (i);
			}
		}
	}

	void RemoveGuests(){
		int c = inn.villagers.Count;
		for (int i = 0; i < c; i++) {
			NPCManager.Instance.RemoveVillager (inn.villagers[0]);
		}
	}

	void createNPC(int i){
		Tile tile = WorldController.Instance.GetTileAt (inn.tile.x + 2 + i, inn.tile.y + 2);

		Vector2 pos = new Vector2 (inn.tile.x + 2 + i, inn.tile.y + 2);

		Villager v = NPCManager.Instance.createVillager (inn,pos);
		v.MovementMode = MoveMode.Idle;

		v.SetHomeAndOffset (inn, tile);
		inn.AddGuest (v);
	}

	void GenerateGameObjects(){
		wallColor = new Color (0, 0, 0);
		floorColor = new Color (0, 0, 255);
		stairsColor = new Color (0, 255, 0);
		counterColor = new Color (255, 0, 255);

		colors = texture.GetPixels ();
		for (int i = 0; i < texture.width; i++) {
			for (int j = 0; j < texture.height; j++) {

				Color c = colors [i + j * texture.width];

				if (c == wallColor) {
					GameObject go = Instantiate (wall);
					Vector2 v = new Vector2 (this.transform.position.x + i, this.transform.position.y + j);
					go.transform.position = v;
					go.transform.SetParent (this.transform);

					WorldController.Instance.SetWalkable ((int)v.x, (int)v.y, 0);

				} else if (c == floorColor) {
					GameObject go = Instantiate (floor);
					go.transform.position = new Vector2 (this.transform.position.x + i, this.transform.position.y + j);
					go.transform.SetParent (this.transform);

				} else if (c == stairsColor) {
					GameObject go1 = Instantiate (floor);
					go1.transform.position = new Vector2 (this.transform.position.x + i, this.transform.position.y + j);
					go1.transform.SetParent (this.transform);

					GameObject go2 = Instantiate (stairs);
					go2.transform.position = new Vector2 (this.transform.position.x + i, this.transform.position.y + j);
					go2.transform.SetParent (this.transform);

				} else if (c == counterColor) {
					GameObject go1 = Instantiate (floor);
					Vector2 v = new Vector2 (this.transform.position.x + i, this.transform.position.y + j);
					go1.transform.position = v;
					go1.transform.SetParent (this.transform);

					GameObject go2 = Instantiate (counter);
					go2.transform.position = new Vector2 (this.transform.position.x + i, this.transform.position.y + j);
					go2.transform.SetParent (this.transform);


					WorldController.Instance.SetWalkable ((int)v.x, (int)v.y, 0);

				}
			}
		}
	}

}
