using UnityEngine;
using System.Collections;

public class SimpleAnimationController : MonoBehaviour {
	public Animator animator;

	public delegate void eventFunction();
	public eventFunction eventfunc;

	public bool start = false;
	// Use this for initialization
	void Start () {

	}

	public void StartAnimation()
	{
		animator.SetTrigger ("Start");
		start = true;
	}


	// Update is called once per frame
	void Update () {
		if(start && animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).IsName("Idle"))
		{
			eventfunc();
			start = false;
		}
	}
}
