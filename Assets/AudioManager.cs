using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager> {

	public int doReMiNum
	{
		get
		{
			return doReMi.Length;
		}
	}
	public AudioClip[] doReMi;
	public AudioClip spawnClip;
	public AudioClip winClip;
	public AudioClip wormOuch;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
