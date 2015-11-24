using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FirstTimePlayer()
	{

	}

	public void LoadMainScene()
	{
		Application.LoadLevel ("Nigel-Halves");
	}

	public void Exit()
	{
		Application.Quit ();
	}
}
