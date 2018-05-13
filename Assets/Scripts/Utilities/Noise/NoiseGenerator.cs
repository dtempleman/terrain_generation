using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour {


	public enum DrawMode{NoiseMap, ColorMap, NormalMap, GameMap};
	public DrawMode drawMode;

	public int width;
	public int height;
	public float scale;

	public int octaves;
	public float persistance;
	public float lacanarity;

	public int seed;
	public Vector2 offset;

	public bool AutoUpdate;

	public TileTerrainType[] regions;

	public void GenerateNoiseMap(){
		float start = Time.realtimeSinceStartup;
		float[,] noiseMap = Noise.GenerateNoiseMap (width,height,seed,scale,octaves,persistance,lacanarity,offset);

		Color[] normalMap = Noise.CreateNormalMap (noiseMap);


		//float[,] noiseMap = Noise.GenerateIslandMask(width, height, 20f);

		Color[] colorMap = new Color[width * height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				float currentHeight = noiseMap [x, y];

				if (currentHeight < .05f) {
					colorMap [y * width + x] = new Color (0, 0, .7f);
	
				} else if (currentHeight < .1f) {
					colorMap [y * width + x] = new Color (0, 0, .8f);

				} else if (currentHeight < .2f) {
					colorMap [y * width + x] = new Color (0, 0, .9f);

				} else if (currentHeight < .3f) {
					colorMap [y * width + x] = new Color (0, 0, 1f);

				} else if (currentHeight < .33f) {
					colorMap [y * width + x] = new Color (1f, 1f, 0);

				} else if (currentHeight < .4f) {
					colorMap [y * width + x] = new Color (0f, 1f, 0f);

				} else if (currentHeight < .55f) {
					colorMap [y * width + x] = new Color (0f, .9f, 0f);

				} else if (currentHeight < .6f) {
					colorMap [y * width + x] = new Color (.5f, .5f, .5f);
		
				}else if (currentHeight < .7f) {
						colorMap [y * width + x] = new Color (.7f, .7f, .7f);

				} else {
					colorMap [y * width + x] = new Color (.97f, .97f, 1f);

				}
			}
		}
		NoiseDisplay display = FindObjectOfType<NoiseDisplay> ();

		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap)); 
		} else if (drawMode == DrawMode.ColorMap) {
			display.DrawTexture (TextureGenerator.TextureFromColorMap (colorMap, width, height)); 
		} else if (drawMode == DrawMode.NormalMap) {
			display.DrawTexture (TextureGenerator.TextureFromColorMap (normalMap, width, height));
		}else if(drawMode == DrawMode.GameMap){
			display.DrawTexture (WorldController.Instance.noiseTexture);
		}

		float end = Time.realtimeSinceStartup;
		Debug.Log ("Generated Map in "+(end-start)+" seconds");

	}

}


[System.Serializable]
public struct TileTerrainType{
	public string name;
	public float height;
	public Color color;
}
