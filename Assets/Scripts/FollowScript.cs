using UnityEngine;
using System.Collections;

public class FollowScript : MonoBehaviour
{

	public Transform target;
	public string compareTag;
	public bool hitHand;
	Transform myTrans;
	// Use this for initialization
	void Start ()
	{
		myTrans = GetComponent<Transform> ();
		myTrans.rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*	myTrans.position = target.position;
		myTrans.rotation = Quaternion.Euler (new Vector3 (0, myTrans.rotation.eulerAngles.y, 0));*/

	}

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag (compareTag)) {
			hitHand = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.CompareTag (compareTag)) {
			hitHand = false;
		}
	}
}
