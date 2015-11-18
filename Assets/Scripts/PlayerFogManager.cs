using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PlayerFogManager : MonoBehaviour
{
	public GlobalFog[]	fogs;
	bool DistanceFog = true, UseRadialDistance = true, HeightFog = true;
	public float height, startDistance ;
	[Range(0.001f,10.0f)]
	public float
		heightDensity;
	public Transform playerTrans;
	public bool enable = true;
	Vector3 playerStart;
	//The distance the player has moved away from the start position
	//And the previous distance the player has moved away from start position
	float distanceFromStart = 0, prevDistanceFromStart = 0;
	bool updateFog = false;
	// Use this for initialization
	void Start ()
	{

		foreach (GlobalFog f in fogs) {
			f.distanceFog = DistanceFog;
			f.useRadialDistance = UseRadialDistance;
			f.heightFog = HeightFog;
		}
		StartCoroutine (UpdateStartDistance ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!updateFog)
			return;

		distanceFromStart = (playerTrans.position - playerStart).magnitude;
		if (prevDistanceFromStart < distanceFromStart) {
			startDistance = distanceFromStart;
			prevDistanceFromStart = distanceFromStart;
		}

		foreach (GlobalFog f in fogs) {
			f.height = height;
			f.heightDensity = heightDensity;
			f.startDistance = startDistance;
		}
	}

	IEnumerator UpdateStartDistance ()
	{
		yield return new WaitForSeconds (10f);
		updateFog = true;
		playerStart = playerTrans.position;
		
		prevDistanceFromStart = distanceFromStart;
	}
}

