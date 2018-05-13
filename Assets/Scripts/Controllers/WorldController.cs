using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour {
	public int SEED = 6;
	public int WIDTH = 1000;
	public int HEIGHT = 1000;

	public static WorldController Instance { get; protected set; }

	World world;
	int[,] walkable;

	public Transform target;


	//public Texture2D worldTexture;
	public int renderDist = 5;

	public Sprite[] grassTiles;
	public Sprite[] waterTiles;
	public Sprite[] sandTiles;
	public Sprite[] stoneTiles;
	public Sprite[] snowTiles;

	Dictionary<Vector2, GameObject> chunkGameObjects;

	int chunkSize;


	Vector2 currrentPos;

	public TilePathingGraph graph;

	public Texture2D noiseTexture;
	public bool useTexture; 

	public bool _draw_graph;


	void Awake(){
		currrentPos = target.transform.position;
		chunkGameObjects = new Dictionary<Vector2, GameObject> ();
		if (Instance != null) {
			Debug.LogError ("there should never be more than 1 npcmanager");
		}
		Instance = this;
		if(useTexture){
			this.WIDTH = noiseTexture.width;
			this.HEIGHT = noiseTexture.height;
		}
		CreateNewWorld ();

	}

	void Start () {
		graph = new TilePathingGraph (world);
		graph.UpdateGraph ();
	}

	void Update () {
		DrawGraph ();
		RefreshWorldChunks ();
	}

	public void CreateNewWorld(){
		walkable = new int[WIDTH , HEIGHT];
		world = new World ();

		float start_time = Time.realtimeSinceStartup;
		noiseTexture = GenerateTexture (WIDTH,HEIGHT,SEED);
		float end_time = Time.realtimeSinceStartup;
		Debug.Log ("Texture created in " + (end_time - start_time) + " seconds.");


		world.SetTexture (noiseTexture);

		chunkSize = world.chunkSize;

		currrentPos = target.transform.position;
		FindChunksToLoad ();
		DeleteOutOfRangeChunks ();
	}

	void RefreshWorldChunks(){
		currrentPos = target.transform.position;
		FindChunksToLoad ();
		DeleteOutOfRangeChunks ();
	}

	void FindChunksToLoad(){
		
		//get current x/y coordinates
		int xPos = (int)currrentPos.x;
		int yPos = (int)currrentPos.y;

		int creationDist = (renderDist-2)*chunkSize;

		for (int i = xPos - creationDist; i < xPos + creationDist; i+= chunkSize) {
			for (int j = yPos - creationDist; j < yPos + creationDist; j+= chunkSize) {

				MakeChunkAt (i, j); 
			}
		}
	}

	void MakeChunkAt(int x, int y){
		x = Mathf.FloorToInt( x / (float) chunkSize ) * chunkSize;
		y = Mathf.FloorToInt( y / (float) chunkSize ) * chunkSize;

		Vector2 cPos = new Vector2 (x, y); 

		if (x >= 0 && y >= 0 && x <= WIDTH && y <= HEIGHT) {

			//if not already loaded
			if (chunkGameObjects.ContainsKey (cPos) == false) {
				Chunk chunk = world.GetChunkAt (x,y);

				if(graph != null){
					graph.UpdateGraph ();
				}

				GameObject go = new GameObject ("Chunk_" + x + "_" + y);
				go.transform.position = cPos;
				go.transform.SetParent (this.transform); 
				FillChunk (chunk, go);
				chunkGameObjects.Add (cPos, go);
			}

		}


	}

	void FillChunk(Chunk chunk, GameObject go){
		for (int i = 0; i < chunk.size; i++) {
			for (int j = 0; j < chunk.size; j++) {

				Tile t = chunk.getTileAt (i, j);
				GameObject tileGO = new GameObject (t.Type + "_Tile_" + (i + chunk.chunkPos.x )+ "_" + (j + chunk.chunkPos.y) + "_" + t.z);


				tileGO.transform.position = new Vector3 (t.x, t.y, t.z);
				tileGO.transform.SetParent (go.transform);
				SpriteRenderer spriteRenderer = tileGO.AddComponent<SpriteRenderer> ();
				spriteRenderer.sprite = getTileSprite (t);
			
				//if the tile is not a water tile it is walkable
				if (t.Type != TileType.Water) {
					walkable [t.x, t.y] = 1; 
				}
			}
		}
	}

	Sprite getTileSprite(Tile t){
		if (t.Type == TileType.Water) {
			return waterTiles [t.z + 4];
		} else if (t.Type == TileType.Sand) {
			return sandTiles [t.z];
		} else if (t.Type == TileType.Grass) {
			return grassTiles [t.z];
		} else if (t.Type == TileType.Stone) {
			return stoneTiles [t.z - 1];
		} else if (t.Type == TileType.Snow) {
			return snowTiles [t.z - 3]; 
		} else {
			return null;
		}
	}

	void DeleteOutOfRangeChunks(){
		List<GameObject> deleteChunks = new List<GameObject> (chunkGameObjects.Values);
		Queue<GameObject> deleteQueue = new Queue<GameObject> ();

		for (int i = 0; i < deleteChunks.Count; i++) {

			float dist = Vector3.Distance (target.transform.position, deleteChunks [i].transform.position);

			if (dist > renderDist * chunkSize) {

				deleteQueue.Enqueue (deleteChunks [i]);

			}
		}

		while (deleteQueue.Count > 0) {
			GameObject chunk = deleteQueue.Dequeue ();

			chunkGameObjects.Remove (chunk.transform.position);

			Destroy (chunk);

		}
	}

	public Tile GetTileAt(int x, int y){
		return world.GetTileAt (x,y);
	}

	public int IsWalkable(int x, int y){
		return walkable [x, y];
	}
	public void SetWalkable(int x, int y, int w){
		walkable [x, y] = w;
	}


	public void DrawGraph(){
		if(!_draw_graph)
			return;
		if(graph != null){
			foreach(PathNode<Tile> t in graph.nodes.Values){
				Vector3 c = new Vector3 (t.data.x + .5f, t.data.y + .5f, 0f);
				foreach(PathEdge<Tile> e in t.edges){
					if(e!=null){
						if(e.node != null){
							Vector3 p = new Vector3 (e.node.data.x + .5f, e.node.data.y + .5f, 0f);
							Debug.DrawLine (c, p, Color.red);
						}
					}
				}
			}
		}
	}



	public Texture2D GenerateTexture(int width, int height, int seed){
		float scale = width/4;
		int octaves = 5;
		float persistance = .5f;
		float lacanarity = 2f;
		Vector2 offset = new Vector2(0f,0f);

		float[,] noiseMap = Noise.GenerateNoiseMap (width,height,seed,scale,octaves,persistance,lacanarity,offset);

		Color[] colorMap = new Color[width * height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				
				float currentHeight = noiseMap [x, y];
				if(useTexture)
					currentHeight = noiseTexture.GetPixel(x,y).b;
				
				if (currentHeight <= world.regions[0]) {
					colorMap [y * width + x] = world.colors[world.regions[0]];

				} else if (currentHeight <= world.regions[1]) {
					colorMap [y * width + x] = world.colors[world.regions[1]];

				} else if (currentHeight <= world.regions[2]) {
					colorMap [y * width + x] = world.colors[world.regions[2]];

				} else if (currentHeight <= world.regions[3]) {
					colorMap [y * width + x] = world.colors[world.regions[3]];

				} else if (currentHeight <= world.regions[4]) {
					colorMap [y * width + x] = world.colors[world.regions[4]];

				} else if (currentHeight <= world.regions[5]) {
					colorMap [y * width + x] = world.colors[world.regions[5]];

				} else if (currentHeight <= world.regions[6]) {
					colorMap [y * width + x] = world.colors[world.regions[6]];

				} else if (currentHeight <= world.regions[7]) {
					colorMap [y * width + x] = world.colors[world.regions[7]];

				} else if (currentHeight <= world.regions[8]) {
					colorMap [y * width + x] = world.colors[world.regions[8]];

				} else {
					colorMap [y * width + x] = world.colors[world.regions[9]];

				}
			}
		}

		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colorMap);
		texture.Apply(); 
		return texture;
	}

}
