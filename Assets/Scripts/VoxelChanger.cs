using UnityEngine;
using System.Collections;

public class VoxelChanger : VoxelParent
{
	/*
	public GameObject model;
	public BIOMES changeBiome;
	public EnvironmentSpawner enviroSpawner;
	BiomeScript biomeScript;

	protected override void Awake ()
	{
		model.SetActive (false);
		base.Awake ();
	}
	
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	
	protected override void allTriggeredEvent ()
	{
		base.allTriggeredEvent ();
		model.SetActive (true);
		
		Vec3Int chunkCoords = vxe.getChunkCoords (transform.position);		
		StartCoroutine (BiomeScript.Instance.resetBiomesThread (chunkCoords.x, chunkCoords.z, (int)changeBiome));
		enviroSpawner.ReplaceSpawnsBasedOnBiome (changeBiome);
		ItemSpawner.Instance.canSpawn = true;
		ItemSpawner.Instance.RestartItemSpawns (this.gameObject);
	}
	
	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
	}*/
}
