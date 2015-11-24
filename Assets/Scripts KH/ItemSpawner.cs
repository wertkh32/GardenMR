using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemInfo
{
	public GameObject item;
	public Material spawnMat;
	public int minSpawnHeightOffFloor;
	public float maxSpawnHeightOffFloor;
}

public class ItemSpawner : Singleton<ItemSpawner>
{
	public Camera camera;
	public ItemInfo[] items;

	[HideInInspector]
	public GameObject[]
		spawneditems;

	[HideInInspector]
	public bool
		canSpawn = true;

	VoxelExtractionPointCloud vxe;
	BiomeScript biome;
	public int currentItemToSpawn = 0;
	int floorChunkY = 0;

	const int range = 2;

	IndexStack<Vec3Int> prevpositions;

	void Start ()
	{
		spawneditems = new GameObject[items.Length];

		for (int i=0; i<items.Length; i++)
			spawneditems [i] = null;

		prevpositions = new IndexStack<Vec3Int> (new Vec3Int[items.Length]);

		vxe = VoxelExtractionPointCloud.Instance;
		biome = BiomeScript.Instance;
		StartCoroutine (SpawnItems ());
	}


	IEnumerator SpawnItems ()
	{
		yield return new WaitForSeconds (10.0f);
		Vector3 coords = Vector3.zero, norm = Vector3.zero;

		bool hitsomething = false;
		while (!hitsomething) {
			hitsomething = vxe.RayCast (camera.transform.position, Vector3.down, 64, ref coords, ref norm, 1.0f);
			yield return null;
		}
		
		floorChunkY = vxe.getChunkCoords (coords).y;
		Vec3Int prevcc = new Vec3Int(vxe.num_chunks_x / 2, vxe.num_voxels_y /2, vxe.num_chunks_z / 2);
		Vector2 prevdir = Vector2.zero;

		for (int i=0; i<items.Length; i++) 
		{

			TriggerScript sheepTrigger = null;

			bool spawned = false;
			while (currentItemToSpawn < items.Length && !spawned) 
			{
				int chunkx;
				int chunkz;
				Random.seed = System.DateTime.Now.Millisecond;
				int attempts = 0;
				while (true) 
				{

					int period = Random.Range (0, vxe.occupiedChunks.getCount ());

					Vec3Int randomCC = vxe.occupiedChunks.peek (period);
					chunkx = randomCC.x;
					chunkz = randomCC.z;

					bool isFarEnough = true;
					for(int k=0;k<prevpositions.getCount();k++)
					{
						Vec3Int pcc = prevpositions.peek(k);
						int dist = (chunkx - pcc.x) * (chunkx - pcc.x) + (chunkz - pcc.z) * (chunkz - pcc.z);
						Debug.Log (dist);
						if(dist < 9)
						{
							isFarEnough = false;
							break;
						}
					}


					if( currentItemToSpawn == 0 || isFarEnough )
					{
						break;
					}

					attempts++;

					if(attempts % 10 == 0)
						yield return null;
				}

				Chunks chunk = null;

				for (int k=floorChunkY + range; k >= floorChunkY; k--) 
				{

					chunk = vxe.grid.voxelGrid [chunkx, k, chunkz];
					Chunks chunkup = vxe.grid.voxelGrid [chunkx, k+1, chunkz];
					bool isthereUp = (chunkup != null && chunkup.voxel_count > 5);

					if (!isthereUp && chunk != null && !chunk.dirty && chunk.voxel_count > 60 && vxe.isChunkASurface(DIR.DIR_UP,chunk,0.6f)) 
					{
						Vector3 chunkBaseCoords = new Vector3 (chunkx, k, chunkz) * vxe.chunk_size;

							
							
						for (int x=0; x<vxe.chunk_size; x++)
							for (int z=0; z<vxe.chunk_size; z++)
								for (int y=vxe.chunk_size-1; y>=0; y--)
							{
									Voxel vx = chunk.getVoxel (new Vec3Int (x, y, z));

									

									if (vx.isOccupied () && vxe.voxelHasSurface (vx, VF.VX_TOP_SHOWN))
									{
										Vector3 voxelCoords = vxe.FromGridUnTrunc (chunkBaseCoords + new Vector3 (x, y, z));
										if (voxelCoords.y < coords.y + items [currentItemToSpawn].minSpawnHeightOffFloor * vxe.voxel_size || voxelCoords.y > coords.y + items [currentItemToSpawn].maxSpawnHeightOffFloor * vxe.voxel_size)
											continue;

										Vector3 position = voxelCoords + new Vector3(vxe.voxel_size * 0.5f, vxe.voxel_size, vxe.voxel_size * 0.5f);
										GameObject newItem = (GameObject)Instantiate (items [currentItemToSpawn].item, position, Quaternion.identity);
										newItem.SetActive (true);

										if(items [currentItemToSpawn].spawnMat!=null)
											vxe.chunkGameObjects[chunkx, k, chunkz].GetComponent<MeshRenderer>().material = items [currentItemToSpawn].spawnMat;
										
										prevpositions.push(new Vec3Int(chunkx,0,chunkz));
										
										currentItemToSpawn++;
										
										spawned = true;
										Debug.Log ("spawned! " + currentItemToSpawn);
										canSpawn = false;
										goto imout;
									}
									yield return null;
							}
					}
				}
					
				imout:
				
				while (!canSpawn)
					yield return null;//new WaitForSeconds (1.0f);
			}
			//Debug.Log ("DONEDONEDONEDONEDONE " + i);
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
		GUI.Label (new Rect (1500, 10, 100, 100), "ITEMS SPAWNED:" + currentItemToSpawn + "Floor chunk: " + floorChunkY);
	}
}
