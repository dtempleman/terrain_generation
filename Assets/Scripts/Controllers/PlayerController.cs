using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed; 
	private Rigidbody2D myRigidBody;


	// Use this for initialization
	void Start () {
		//movement
		myRigidBody = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {

		//go up/down
		if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw ("Vertical") < -0.5f) {
			transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * speed * Time.deltaTime, 0f));
		}

		//go left/right
		if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < -0.5f) {
			transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0f, 0f));
		}
		//stop moving is no button pressed
		if(Input.GetAxisRaw("Horizontal")==0 && Input.GetAxisRaw("Vertical")==0){
			myRigidBody.velocity = new Vector2(0f,0f);
		}
	}

}
