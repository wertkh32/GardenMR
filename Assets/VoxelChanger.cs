using UnityEngine;
using System.Collections;

public class VoxelChanger : VoxelParent
{
	
	public GameObject model;
	public BIOMES changeBiome;
	BiomeScript biomeScript;

	protected override void Awake ()
	{
		model.SetActive (false);
		base.Awake ();
	}
	
	// Use this for initialization
	protected override void Start ()
	{
		biomeScript = BiomeScript.Instance;
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	
	IEnumerator fall ()
	{
		Vector3 coords = Vector3.zero, norm = Vector3.zero;
		bool hit = vxe.RayCast (transform.position, Vector3.down, 64, ref coords, ref norm, 0.5f);

		Vector3 startpos = transform.position;
		coords.x = startpos.x;
		coords.z = startpos.z;
		if (hit) {
			for (float i=0; i<0.5f; i+= Time.deltaTime) {
				transform.position = Vector3.Lerp (startpos, coords, i * 2);
				Debug.Log ("falling");
				yield return null;
			}
		}

		Vec3Int chunkCoords = vxe.getChunkCoords (transform.position);
		StartCoroutine (biomeScript.resetBiomesThread (chunkCoords.x, chunkCoords.z, (int)changeBiome));

	}
	
	protected override void allTriggeredEvent ()
	{
		base.allTriggeredEvent ();
		model.SetActive (true);
		StartCoroutine (fall ());
		ItemSpawner.Instance.canSpawn = true;
		
	}
	
	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
	}
}
