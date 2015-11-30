using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemInfo
{
	public GameObject item;
	public BIOMES biome;
	public int minSpawnHeightOffFloor;
	public float maxSpawnHeightOffFloor;
}

[System.Serializable]
public class SpawnStage
{
	public ItemInfo[] items;
	public bool allAtOnce;
	public bool stageWait;
}

public class ItemSpawner : Singleton<ItemSpawner>
{
	public Camera camera;
	public SpawnStage[] stages;

	//[HideInInspector]
	//public GameObject[]
	//	spawneditems;

	[HideInInspector]
	public int currentStage = 0;

	[HideInInspector]
	public bool canSpawn = true;

	[HideInInspector]
	public bool nextStage = false;

	VoxelExtractionPointCloud vxe;
	BiomeScript biome;
	public int SpawnCount = 0;
	int floorChunkY = 0;

	const int range = 2;

	IndexStack<Vec3Int> prevpositions;


	void Start ()
	{

		//spawneditems = new GameObject[items.Length];


		//for (int i=0; i<items.Length; i++)
		//	spawneditems [i] = null;

		prevpositions = new IndexStack<Vec3Int> (new Vec3Int[20]);

		vxe = VoxelExtractionPointCloud.Instance;
		if (camera == null)
			camera = vxe.camera;
		biome = BiomeScript.Instance;

	}

	public void startSpawining()
	{
		StartCoroutine (SpawnItems ());
	}


	IEnumerator SpawnItems ()
	{
		yield return new WaitForSeconds (5.0f);
		Vector3 coords = Vector3.zero, norm = Vector3.zero;

		bool hitsomething = false;
		while (!hitsomething) {
			hitsomething = vxe.RayCast (camera.transform.position, Vector3.down, 64, ref coords, ref norm, 1.0f);
			yield return null;
		}

		Random.seed = System.DateTime.Now.Millisecond;
		floorChunkY = vxe.getChunkCoords (coords).y;
		Vec3Int prevcc = new Vec3Int (vxe.num_chunks_x / 2, vxe.num_voxels_y / 2, vxe.num_chunks_z / 2);
		Vector2 prevdir = Vector2.zero;

		for(int s=0;s<stages.Length;s++)
		{
			currentStage = s;
			ItemInfo[] items = stages[s].items;
			nextStage = false;

			for (int i=0; i<items.Length; i++) 
			{
				int biomeIndex = (int)items[i].biome;
				IndexStack<Vec3Int> occupiedChunks = 
					items[i].biome == BIOMES.none ? vxe.occupiedChunks : BiomeScript.Instance.biomeOccupiedChunks[biomeIndex];

				bool spawned = false;
				int maxdist = 3;
				while (!spawned) 
				{
					int chunkx;
					int chunkz;
					int attempts = 0;
					while (true) 
					{
						int count = occupiedChunks.getCount ();
						int period = Random.Range (0, count);
						
						Vec3Int randomCC = occupiedChunks.peek (period);
						chunkx = randomCC.x;
						chunkz = randomCC.z;


						bool isFarEnough = true;
						int start = Mathf.Max(0, prevpositions.getCount() - 3);

						for(int k=start;k<prevpositions.getCount();k++)
						{
								Vec3Int pcc = prevpositions.peek(k);
								int dist = (chunkx - pcc.x) * (chunkx - pcc.x) + (chunkz - pcc.z) * (chunkz - pcc.z);
								Debug.Log (dist);
								if(dist < maxdist * maxdist)
								{
									isFarEnough = false;
									break;
								}
						}
							
							
						if( SpawnCount == 0 || isFarEnough )
						{
								break;
						}
							
						attempts++;
							
						if(attempts % 10 == 0)
						{
								if(maxdist > 1)
									maxdist--;
						}

						yield return null;
					}



					Chunks chunk = null;

					for (int k=floorChunkY + range; k >= floorChunkY; k--) {

						chunk = vxe.grid.voxelGrid [chunkx, k, chunkz];
						Chunks chunkup = vxe.grid.voxelGrid [chunkx, k + 1, chunkz];
						bool isthereUp = (chunkup != null && chunkup.voxel_count > 3);

						if (!isthereUp && chunk != null && chunk.voxel_count > 60 && vxe.isChunkASurface (DIR.DIR_UP, chunk, 0.6f)) {
							Vector3 chunkBaseCoords = new Vector3 (chunkx, k, chunkz) * vxe.chunk_size;


								
								
							for (int ox=0; ox<vxe.chunk_size; ox++)
							{
								int x = (ox + vxe.chunk_size / 2 - 1) % vxe.chunk_size;
								for (int oz=0; oz<vxe.chunk_size; oz++)
								{
									int z = (oz + vxe.chunk_size / 2 - 1) % vxe.chunk_size;
									for (int y=vxe.chunk_size-1; y>=0; y--) 
									{
										
										Voxel vx = chunk.getVoxel (new Vec3Int (x, y, z));

										if (vx.isOccupied () && vxe.voxelHasSurface (vx, VF.VX_TOP_SHOWN)) 
										{
											Vector3 voxelCoords = vxe.FromGridUnTrunc (chunkBaseCoords + new Vector3 (x, y, z));
											if (voxelCoords.y < coords.y + items [i].minSpawnHeightOffFloor * vxe.voxel_size || 
											    voxelCoords.y > coords.y + items [i].maxSpawnHeightOffFloor * vxe.voxel_size)
												continue;

											GameObject newItem = (GameObject)Instantiate (items [i].item, voxelCoords + new Vector3 (0, vxe.voxel_size, 0), Quaternion.identity);
											newItem.SetActive (true);


											newItem.GetComponent<VoxelParent>().chunkCoords = new Vec3Int(chunkx,k,chunkz);

											prevpositions.push(new Vec3Int(chunkx,0,chunkz));

											SpawnCount++;
											
											spawned = true;
											Debug.Log ("spawned!");
											canSpawn = false;
											goto imout;
										}
										yield return null;
								 	}
								}
							}
						}
					}
						
					imout:
					
					if(spawned && !stages[s].allAtOnce)
						while (!canSpawn)
							yield return new WaitForSeconds (1.0f);
					else
						yield return null;
				}
				

			}

			while(stages[s].stageWait && !nextStage)
				yield return new WaitForSeconds (1.0f);
		}
	}


	/*public IEnumerator DropFirstSheepBush (GameObject pocketWatch, GameObject Spawn1stSheep)
	{
		Vector3 vxCoord = Vector3.zero, normal = Vector3.zero;
		bool hit = false;
		
		//Leave this here to make give ItemSpawner time to maker sure vxe is not null
		if (vxe == null)
			vxe = VoxelExtractionPointCloud.Instance;

		yield return new WaitForEndOfFrame ();

		while (!hit) {
			hit = vxe.RayCast (pocketWatch.transform.position, Vector3.down, 64f, ref vxCoord, ref normal, 1f);
			yield return null;
		}
		items [currentItemToSpawn].item.transform.position = vxCoord + Vector3.up * vxe.voxel_size * 1.0f;
		currentItemToSpawn++;
		Debug.Log ("spawned!");
		canSpawn = false;

	}*/
	
	
	void OnGUI ()
	{
		GUI.Label (new Rect (1500, 10, 100, 100), "ITEMS SPAWNED:" + SpawnCount + "Floor chunk: " + floorChunkY);
	}
}
