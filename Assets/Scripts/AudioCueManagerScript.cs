using UnityEngine;
using System.Collections;

public class AudioCueManagerScript : MonoBehaviour
{
	public AudioSource au_source;

	public AudioClip countingSheep, goFindSheep, goodJob, imPuttingYouToSleep, lookForPocketWatch;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void playAudioClip (AudioClip clip, bool loop=false)
	{
		au_source.Stop ();
		au_source.clip = clip;
		au_source.loop = loop;
		au_source.Play ();
	}
}
