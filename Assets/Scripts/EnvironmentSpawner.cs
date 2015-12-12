using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Environment
{
	GameObject[] gameObjects;
	Dictionary<Vec3Int,GameObject> table;
}



public class EnvironmentSpawner: Singleton<EnvironmentSpawner>
{
	Vector3[] directions = {
		new Vector3 (-1, 0, 0),
		new Vector3 (1, 0, 0),
		new Vector3 (0, 0, -1),
		new Vector3 (0, 0, 1),
		new Vector3 (-1, 0, 1),
		new Vector3 (1, 0, 1),
		new Vector3 (-1, 0, -1),
		new Vector3 (1, 0, -1),
	};

	public int spawnInterval = 30, minVoxelCount = 10;
	public float surfaceThreshold = 0.6f;
	public Transform playerTrans;
	public GameObject desertGameObjects, grassGameObjects, marshGameObjects;
	public int max_spawns = 200;
	int spawnCount = 0;
	public IndexStack<SpawnObject> spawns;
	//May want to use public string tags
	string desertTag, grassTag, IceTag, marshTag;
	
	VoxelExtractionPointCloud vxe;
	BiomeScript biome;
	BIOMES mybiome;

	public GameObject[] desertAssets, grassAssets, marshAssets;
	Dictionary<Vec3Int,GameObject> assetChunkTable;

	int framecount = 0;
	Chunks chunk;
	bool isSurface, inHashTable;
	List<Chunks> occupiedNearMe;
	Vec3Int playerCC;
	int chunkx, chunkz;
	bool isSpawning = false;
	// Use this for initialization
	void Start ()
	{
		vxe = VoxelExtractionPointCloud.Instance;
		biome = BiomeScript.Instance;
		mybiome = biome.biomeArea [0].biome;
		occupiedNearMe = new List<Chunks> ();
		assetChunkTable = new Dictionary<Vec3Int, GameObject> ();

		if (playerTrans == null)
			playerTrans = GameObject.FindWithTag ("Player").GetComponent<Transform> ();

		spawns = new IndexStack<SpawnObject> (new SpawnObject[max_spawns + 20]);
		StartCoroutine (FullPullSpawn ());
		StartCoroutine (MaintainSpawns ());
	}


	/// <summary>
	/// Waits to initialize.
	/// </summary>
	/// <returns>The to initialize.</returns>
	IEnumerator waitToInitialize ()
	{
		while (biome.biomeMap == null) {
			yield return new WaitForEndOfFrame ();
		}
		playerCC = vxe.getChunkCoords (playerTrans.position);
		mybiome = biome.biomeMap [playerCC.x, playerCC.z];
	}
	
	// Update is called once per frame
	void Update ()
	{
		framecount++;
	}

	/// <summary>
	/// Sets the active.
	/// </summary>
	/// <param name="active">If set to <c>true</c> active.</param>
	void SetActive (bool active)
	{
		desertGameObjects.SetActive (active);
		marshGameObjects.SetActive (active);
		grassGameObjects.SetActive (active);
	}

	/// <summary>
	/// Sets the environment list based on biome.
	/// </summary>
	/// <returns>The environment list based on biome.</returns>
	/// <param name="Pos">Position.</param>
	GameObject[] SetEnvironmentListBasedOnBiome (Vector3 Pos)
	{
		playerCC = vxe.getChunkCoords (Pos);
		chunkx = playerCC.x;
		chunkz = playerCC.z;
		
		mybiome = biome.biomeMap [chunkx, chunkz];
		return getAssetListBasedOnBiome (mybiome);
	}

	/// <summary>
	/// Gets environment list based on biome.
	/// </summary>
	/// <returns>The environment list based on biome.</returns>
	/// <param name="Pos">Position.</param>
	GameObject[] GetEnvironmentListBasedOnBiome (Vec3Int Pos)
	{
		return getAssetListBasedOnBiome ((biome.getBiomeFromCoords (Pos)));

		//return getNextEnvironmentGameObject (biome.biomeMap [Pos.x, Pos.z]);
	}

	/// <summary>
	/// Does a the whole pull spawn process in one place.
	/// </summary>
	/// <returns>The pull spawn.</returns>
	IEnumerator FullPullSpawn ()
	{
		Vector3 chunkBaseCoords;
		GameObject[] assetList;
		
		while (true) {
			for (int i =vxe.occupiedChunks.getCount() - 1; i>-1; i--) {
			
				//Chunk Voxel Coordinates from the Occupied Chunk Stack
				Vec3Int chunkVXCoords = vxe.occupiedChunks.peek (i);

				//Get the Correct Environment Assets based on Biome
				assetList = GetEnvironmentListBasedOnBiome (chunkVXCoords);

				//Chunks World Coords

				chunk = vxe.grid.voxelGrid [chunkVXCoords.x, chunkVXCoords.y, chunkVXCoords.z];

				if (chunk == null || assetList.Length == 0) {
					continue;
				}

				if (spawnCount > max_spawns)
					yield break;
				//If the chunk is a surface and is not in the HashTable, do Spawning code
				if (!chunk.spawnPopulated && chunk.voxel_count > minVoxelCount) {

					if (Random.Range (0, 3) == 0) {
						GameObject gbj = assetList [Random.Range (0, assetList.Length)];
						onManyVoxelPosition (chunk, chunkVXCoords, gbj);
					}

					chunk.spawnPopulated = true;

				}   
				yield return null;
			}
			yield return new WaitForSeconds (1.0f);
		}
	}

	IEnumerator MaintainSpawns ()
	{
		while (true) {
			int counter = 0;
			for (int i=0; i<spawns.getCount(); i++) {
				spawns.peek (i).checkWeirdPosition ();
				counter++;
				//This makes sure that  only 3 environment spawns are checked every frame at a time (instead of the entire list of spawns)
				if (counter % 5 == 0)
					yield return null;
			}
			yield return null;
		}
	}

	public void endGameSwitchSpawns ()
	{
		StartCoroutine (SwitchOutSpawns ());
	}

	IEnumerator SwitchOutSpawns ()
	{
		while (true) {
			int counter = 0;
			for (int i=0; i<spawns.getCount(); i++) {

				if (!spawns.peek (i).switched)
					spawns.peek (i).switchOutAlt ();

				counter++;
				yield return null;
			}
			yield return null;
		}
	}

	/// <summary>
	/// Gets the next environment game objectList
	/// </summary>
	/// <returns>The next environment game object.</returns>
	/// <param name="newBiome">New biome.</param>
	GameObject[] getAssetListBasedOnBiome (BIOMES newBiome)
	{
		switch (newBiome) {
		case BIOMES.grass:
			return	grassAssets;
		case BIOMES.sand:
			return desertAssets;
		default: //marsh
			return marshAssets;
		}
	}

	/// <summary>
	/// Spawns a GameObject on many voxels in a chunk.
	/// </summary>
	/// <param name="chunk">Chunk.</param>
	/// <param name="chunkGridCoord">Chunk grid coordinate.</param>
	/// <param name="obj">Object.</param>
	void onManyVoxelPosition (Chunks chunk, Vec3Int chunkGridCoord, GameObject obj)
	{
		Vector3 chunkBCoords = new Vector3 (chunkGridCoord.x, chunkGridCoord.y, chunkGridCoord.z) * vxe.chunk_size;

		Random.seed = (int)Time.frameCount;

		for (int x=0; x<vxe.chunk_size; x++)
			for (int z=0; z<vxe.chunk_size; z++) {
				for (int y=vxe.chunk_size-1; y>=0; y--) {
					Voxel vx = chunk.getVoxel (new Vec3Int (x, y, z));
					
					if (vx.isOccupied () && vxe.voxelHasSurface (vx, VF.VX_TOP_SHOWN)) {
						Vector3 voxelCoords = vxe.FromGridUnTrunc (chunkBCoords + new Vector3 (x, y, z));

						if (Random.Range (0, 20) == 0) {
							GameObject newObj = Instantiate (obj, voxelCoords + Vector3.up * vxe.voxel_size * 1.0f, Quaternion.AngleAxis (Random.Range (0, 360), Vector3.up)) as GameObject;
							newObj.transform.parent = obj.transform.parent;	
							newObj.SetActive (true);
							spawns.push (newObj.GetComponent<SpawnObject> ());
							spawnCount++;
							return;
						}						
					}
				}
			}
	}


}
