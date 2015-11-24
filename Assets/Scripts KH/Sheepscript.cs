using UnityEngine;
using System.Collections;

public class Sheepscript : VoxelParent {
	public GameObject sheepModel;
	public SimpleAnimationController ani;
	BiomeScript biomeScript;

	protected override void Awake ()
	{
		sheepModel.SetActive (false);
		base.Awake ();
	}
	
	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		biomeScript = BiomeScript.Instance;
		Vec3Int chunkCoords = vxe.getChunkCoords (transform.position);
		vxe.chunkGameObjects [chunkCoords.x,chunkCoords.y,chunkCoords.z].GetComponent<MeshRenderer> ().material = vxe.debugMaterial;
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();
	}
	
	protected override void allTriggeredEvent ()
	{
		base.allTriggeredEvent ();
		sheepModel.SetActive (true);
		ani.eventfunc = startAI;
		ani.NextAnimation ();
		ItemSpawner.Instance.canSpawn = true;
		
	}

	public void startAI()
	{
		sheepModel.GetComponent<JumpingAI> ().init ();
	}
	
	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
		
	}
}
