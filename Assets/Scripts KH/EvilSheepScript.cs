using UnityEngine;
using System.Collections;

public class EvilSheepScript : VoxelParent
{

	public GameObject sheepModel;
	public SimpleAnimationController ani;
	public Material ruinTexture;
	public GameObject[] plantToSpawn;

	public bool isBoss = false;
	public GameObject endMessage;

	protected override void Awake ()
	{
		//sheepModel.SetActive (false);
		base.Awake ();
	}
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		//startAI ();
		//evil sheep definitely do this.
		ruinTerrain ();
		StartCoroutine (PlayAudioLoop (AudioManager.Instance.wormEating, AudioManager.Instance.wormEating));
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	
	protected override void allTriggeredEvent ()
	{
		base.allTriggeredEvent ();
		partsys.gameObject.transform.parent = transform;
		partsys.startLifetime = 1;
		partsys.startColor = Color.red;
		partsys.startSpeed = 1.0f;
		partsys.startSize = 0.3f;
		partsys.maxParticles = 100;
		partsys.Clear ();
		partsys.Stop ();
		partsys.Emit (100);

		//if (!isBoss)
		StartCoroutine (PlayAllTriggerAudio (AudioManager.Instance.wormOuch));
		//else
		//	StartCoroutine (PlayAllTriggerAudio (AudioManager.Instance.winClip));
		ani.eventfunc = destroyWorm;
		ani.NextAnimation ();
	}

	public void destroyWorm ()
	{

		sheepModel.SetActive (false);

		if (isBoss) {
			GameObject newItem = (GameObject)Instantiate (plantToSpawn [0], transform.position + Vector3.up * vxe.voxel_size * 5, Quaternion.identity);
			newItem.SetActive (true);
			newItem.GetComponent<VoxelParent> ().chunkCoords = chunkCoords;
			newItem.GetComponent<BushScript>().popEventHandler = endGame;

		} else {
			int plant = (int)BiomeScript.Instance.biomeMap [chunkCoords.x, chunkCoords.z];
			GameObject newItem = (GameObject)Instantiate (plantToSpawn [plant], transform.position + Vector3.up * vxe.voxel_size * 4, Quaternion.identity);
			newItem.SetActive (true);
			newItem.GetComponent<VoxelParent> ().chunkCoords = chunkCoords;
		}
	}

	public void endGame()
	{
		EnvironmentSpawner.Instance.endGameSwitchSpawns ();
		endMessage.SetActive (true);

	}

	public void startAI ()
	{
		//GetComponent<JumpingAI> ().init ();
	}

	public void ruinTerrain ()
	{
		vxe.changeChunkMaterial (chunkCoords, ruinTexture);
	}
	
	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
		
	}
}
