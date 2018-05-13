using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

	public static NPCManager Instance { get; protected set; }

	public GameObject villager;

	private List<Villager> AvailableVillagersData;
	public  List<Villager> VillagersData { get; protected set; } 

	private Dictionary<Villager,GameObject> VillagerGameObjcts;

	void Awake(){
		AvailableVillagersData = new List<Villager> ();
		VillagersData = new List<Villager> ();
		VillagerGameObjcts = new Dictionary<Villager, GameObject> ();
	}

	// Use this for initialization
	void Start () {
		if (Instance != null) {
			Debug.LogError ("there should never be more than 1 npcmanager");
		}
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Villager createVillager(Building home, Vector2 pos){
		Villager data = new Villager (home);

		GameObject go = Instantiate (villager);
		go.transform.position = pos;
		go.transform.SetParent (this.transform);

		VillagerController vc = go.AddComponent<VillagerController> ();
		vc.AddVillagerData (data);
		vc.AddHomeTile (pos);

		AvailableVillagersData.Add (data);
		VillagerGameObjcts.Add (data,go);
		return data;
	}

	public void RemoveVillager (Villager v){
		v.home.RemoveVillager (v); 
		if (VillagersData.Contains (v)) {
			VillagersData.Remove (v); 
		}
		if (AvailableVillagersData.Contains (v)) {
			AvailableVillagersData.Remove (v); 
		}
		if (VillagerGameObjcts.ContainsKey (v)) {
			Destroy (VillagerGameObjcts [v]);
			VillagerGameObjcts.Remove (v); 
		}
	}

	public Villager FindAvailableWorker(Building home, Vector2 offset){
		if (AvailableVillagersData.Count > 0) {
			Villager v = AvailableVillagersData[0];
			AvailableVillagersData.Remove (v);
			v.home.RemoveVillager (v);
			Tile t = WorldController.Instance.GetTileAt (home.tile.x + (int)offset.x, home.tile.y + (int)offset.y);
			v.SetHomeAndOffset (home,t);
			VillagersData.Add (v);
			return v;
		}
		return null; 
	}

}
