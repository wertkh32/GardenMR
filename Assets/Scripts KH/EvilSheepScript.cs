using UnityEngine;
using System.Collections;

public class EvilSheepScript : VoxelParent {

	public GameObject sheepModel;
	public SimpleAnimationController ani;
	public Material ruinTexture;
	public GameObject plantToSpawn;
	
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

		audioSource.PlayOneShot (AudioManager.Instance.wormOuch);

		ani.eventfunc = destroyWorm;
		ani.NextAnimation ();


	}

	public void destroyWorm()
	{
		sheepModel.SetActive (false);
		GameObject newItem = (GameObject)Instantiate (plantToSpawn, transform.position + Vector3.up * vxe.voxel_size * 4, Quaternion.identity);
		newItem.SetActive (true);
		newItem.GetComponent<VoxelParent> ().chunkCoords = chunkCoords;
	}

	public void startAI()
	{
		//GetComponent<JumpingAI> ().init ();
	}

	public void ruinTerrain()
	{
		vxe.changeChunkMaterial(chunkCoords, ruinTexture);
	}
	
	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
		
	}
}
