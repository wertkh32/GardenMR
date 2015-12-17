using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO; 
using System.Text;

public class UIScript : MonoBehaviour 
{
	public Animator startAnimator;
	public SimpleAnimationController control;
	public Button startButton;
	string filename = "settings.txt";
	FileInfo fileinfo;
	bool isFirstTime = true;

	void Awake()
	{
		control = GetComponent<SimpleAnimationController> ();
		fileinfo = new FileInfo (Application.persistentDataPath + "\\" + filename);
		Debug.Log (Application.persistentDataPath + "\\" + filename);
		isFirstTime = !fileinfo.Exists;
		if(isFirstTime)
		{
			startButton.interactable = false;
		}
		else
		{
			startButton.interactable = true;
		}
	}
	// Use this for initialization
	void Start () 
	{

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
		if(isFirstTime)
		{
			StreamWriter w;
			w = fileinfo.CreateText();
			w.WriteLine("Created");
			w.Close();
		}

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

	public void openDisclaimer()
	{
		Application.OpenURL("http://www.etc.cmu.edu/projects/gotan/index.php/warning/");
	}

	public void Exit()
	{
		Application.Quit ();
	}
}
