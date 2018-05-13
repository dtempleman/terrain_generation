using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class MouseController : MonoBehaviour {

	public Movement target;

	public GameObject CursorPrefab;




	//the world-position of the mouse last frame.
	//Vector3 LastFramePosition;
	Vector3 CurrentFramePosition;



	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		CurrentFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if(Input.GetMouseButtonUp(0)){
			target.SetManualDest ((int)CurrentFramePosition.x, (int)CurrentFramePosition.y);
		}
			
	}

}


