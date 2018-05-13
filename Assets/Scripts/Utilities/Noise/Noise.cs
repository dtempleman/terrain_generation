using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise{

	public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistance, float laconarity, Vector2 offset){

		float[,] noiseMap = new float[width, height];

		float[,] mask = GenerateIslandMask (width, height);

		System.Random prng = new System.Random (seed);
		Vector2[] octavesOffsets = new Vector2[octaves];

		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000);
			float offsetY = prng.Next (-100000, 100000);
			octavesOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				
				float amplitude = 1;
				float frequescy = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {

					float sampleX = x/scale * frequescy + octavesOffsets[i].x + offset.x;
					float sampleY = y/scale * frequescy + octavesOffsets[i].y + offset.y;

					float PerlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;

					noiseHeight += PerlinValue * amplitude;

					amplitude *= persistance; 
					frequescy *= laconarity;
				}

				if(noiseHeight > maxNoiseHeight){
					maxNoiseHeight = noiseHeight;
				}
				if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}

				noiseMap [x, y] = noiseHeight;
			}
		}

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
				noiseMap [x, y] = noiseMap [x, y] * mask [x, y];
			}
		}
			
		return noiseMap;
	}

	public static float[,] GenerateIslandMask(int width, int height){
		
		float[,] mask = new float[width,height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				float tempX = Mathf.Abs (x - (width * .5f));
				float tempY = Mathf.Abs (y - (height * .5f));
				float dist = Mathf.Sqrt (Mathf.Pow (tempX, 2) + Mathf.Pow (tempY, 2));

				float max_width = (width * .6f) - 10f;
				//float max_width = (width * .6f) - 10f;

				float delta = dist / max_width; 
				float gradient = Mathf.Pow (delta, 2); 
		
				mask [x, y] = Mathf.Max (.05f, 1f - gradient);

			}
		}
		return mask;
	}
		
	public static Color[] CreateNormalMap(float[,] HeightMap){
		int width = HeightMap.GetLength (0);
		int height = HeightMap.GetLength (1);

		Color[] ColorMap = new Color[width * height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				ColorMap [y * width + x] = GetNormalOfPoint(x, y, HeightMap);

			}
		}
		return ColorMap;
	}

	public static Color GetNormalOfPoint(int x, int y, float[,] HeightMap){
		
		if (x == 0 || x >= HeightMap.GetLength (0)-1 || y == 0 || y >= HeightMap.GetLength (1)-1)
			return new Color (0, 255, 0);
		float a = HeightMap.GetLength(0)/20f;

		float z1 = a * (HeightMap [x + 1, y] - HeightMap [x - 1, y]);
		float z2 = a * (HeightMap [x, y + 1] - HeightMap [x, y - 1]);

		Vector3 N = new Vector3 (z2, z1, 1); 

		N.Normalize ();
		Color C = new Color(N.x, N.y, N.z);
		return C;
	}

}
