using UnityEngine;
using System.Collections;

public class BillBoardScript : MonoBehaviour {
	public Camera camera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = camera.transform.forward;
		dir = Vector3.ProjectOnPlane (dir, Vector3.up);
		this.transform.forward = dir;
	}
}
