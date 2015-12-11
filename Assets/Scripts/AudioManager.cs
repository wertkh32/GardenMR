using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{

	public int doReMiNum {
		get {
			return doReMi.Length;
		}
	}
	public AudioClip[] doReMi;
	public AudioClip spawnClip, spawnLoopClip;
	public AudioClip winClip;
	public AudioClip wormOuch, wormEating;
	public AudioClip endGameClip;

	public void play2DSound(AudioClip clip)
	{
		GetComponent<AudioSource> ().PlayOneShot (clip);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
