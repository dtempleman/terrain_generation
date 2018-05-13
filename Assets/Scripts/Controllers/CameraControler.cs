using UnityEngine;
using System.Collections;

public class CameraControler : MonoBehaviour {
	public Transform target;
	Camera myCam;
	// Use this for initialization
	void Start () {
		myCam = GetComponent<Camera> ();
		if(this.transform.tag!="MainCamera"){
			myCam.orthographicSize = 25f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(this.transform.tag=="MainCamera"){
			myCam.orthographicSize -= myCam.orthographicSize * Input.GetAxis ("Mouse ScrollWheel") * 1.5f;
			myCam.orthographicSize = Mathf.Clamp (myCam.orthographicSize, 2f, 15f);
		}


		if (target) {
			transform.position = Vector3.Lerp(transform.position, target.position, .2f) + new Vector3(0,0,-5);
		}
	}
}
