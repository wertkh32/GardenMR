using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour 
{
	public Animator startAnimator;
	public SimpleAnimationController control;
	void Awake()
	{
		control = GetComponent<SimpleAnimationController> ();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FirstTimePlayer()
	{

	}

	public void LoadMainSceneDirect()
	{
		Application.LoadLevel ("Nigel-Halves");
	}

	public void LoadMainScene()
	{
		control.eventfunc = LoadMainSceneDirect;
		control.StartAnimation ();
	}

	public void Exit()
	{
		Application.Quit ();
	}
}
