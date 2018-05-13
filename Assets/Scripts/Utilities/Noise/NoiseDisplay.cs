using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDisplay : MonoBehaviour {

	public Renderer textureRenederer;

	public void DrawTexture(Texture2D texture){
		textureRenederer.sharedMaterial.mainTexture = texture;
		textureRenederer.transform.localScale = new Vector3 (texture.width, texture.height, texture.height);
	}
}
