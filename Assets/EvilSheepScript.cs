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
		GetComponent<AudioSource> ().Play ();
		sheepModel.SetActive (false);
		ItemSpawner.Instance.canSpawn = true;

		GameObject newItem = (GameObject)Instantiate (plantToSpawn, transform.position + Vector3.up * vxe.voxel_size * 4, Quaternion.identity);
		newItem.SetActive (true);
	}
	
	public void startAI()
	{
		//GetComponent<JumpingAI> ().init ();
	}

	public void ruinTerrain()
	{
		Vec3Int cc = vxe.getChunkCoords (transform.position);
		for(int i=0;i<vxe.num_chunks_y;i++)
		{
			vxe.chunkGameObjects[cc.x,i,cc.z].GetComponent<MeshRenderer>().material = ruinTexture;
		}
	}
	
	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
		
	}
}
