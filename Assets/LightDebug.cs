using UnityEngine;
using System.Collections;

public class LightDebug : MonoBehaviour {
	public float front_diffuse;
	public float back_diffuse;
	public float left_diffuse;
	public float right_diffuse;
	public float up_diffuse;
	public float down_diffuse;
	public string arraystr;

	//DIR_FRONT = 0,
	//DIR_BACK = 1,
	//DIR_LEFT = 2,
	//DIR_RIGHT = 3,
	//DIR_UP = 4,
	//DIR_DOWN = 5,
	// Use this for initialization
	void Start () {
		Vector3 lightDir = -Vector3.Normalize (transform.forward);
		front_diffuse = Mathf.Max (0.0f, Vector3.Dot ( Vector3.forward, lightDir ));
		back_diffuse = Mathf.Max (0.0f, Vector3.Dot ( Vector3.back, lightDir ));
		left_diffuse = Mathf.Max (0.0f, Vector3.Dot ( Vector3.left, lightDir ));
		right_diffuse = Mathf.Max (0.0f, Vector3.Dot ( Vector3.right, lightDir ));
		up_diffuse = Mathf.Max (0.0f, Vector3.Dot ( Vector3.up, lightDir ));
		down_diffuse = Mathf.Max (0.0f, Vector3.Dot ( Vector3.down, lightDir ));
		arraystr = front_diffuse + ", " +
			back_diffuse + ", " +
			left_diffuse + ", " +
			right_diffuse + ", " +
			up_diffuse + ", " +
			down_diffuse;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
