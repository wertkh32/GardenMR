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

	public IEnumerator PlayStreamingVideo(string url)
	{
		Handheld.PlayFullScreenMovie(url, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		OnIntroDone ();
	}
	
	public void FirstTimePlayer()
	{
		StartCoroutine(PlayStreamingVideo("intro.mp4"));
	}

	public void OnIntroDone()
	{
		gotoDisclaimer ();
	}

	public void LoadMainSceneDirect()
	{
		Application.LoadLevel ("Nigel-Halves");
	}

	public void LoadMainScene()
	{
		control.eventfunc = LoadMainSceneDirect;
		control.NextAnimation ();
	}

	public void gotoDisclaimer()
	{
		control.NextAnimation ();
	}

	public void Exit()
	{
		Application.Quit ();
	}
}
