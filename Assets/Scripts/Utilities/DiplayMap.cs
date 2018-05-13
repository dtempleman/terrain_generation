using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiplayMap : MonoBehaviour {

	Texture2D worldMap;

	// Use this for initialization
	void Start () {
		worldMap = WorldController.Instance.noiseTexture;
		SpriteRenderer sr = this.GetComponent<SpriteRenderer> ();
		Sprite worldMapSprite = Sprite.Create (worldMap
												,new Rect (
													new Vector2(0,0)
													,new Vector2(worldMap.width, worldMap.height))
												,new Vector2 (0, 0)
												,1
												);
		sr.sprite = worldMapSprite; 
		Color c = sr.color;
		c.a = .25f;
		sr.color = c;
	}
}
