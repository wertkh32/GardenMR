using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BIOMES : int
{
	grass = 0,
	sand = 1,
	jungle = 2,
	none = 3
}

public class BiomeScript : Singleton<BiomeScript>
{
	VoxelExtractionPointCloud vxe;
	GameObject[,,] chunkObjs;

	const int activeBiomes = 3;

	[System.Serializable]
	public class BiomeArea
	{
		public int radius;
		public BIOMES biome;
		public Vector2 position;
	}

	public BiomeArea[] biomeArea;

	[System.Serializable]
	public struct BiomeSet
	{
		public Material[] materials;
		public BIOMES biome;
		public Material getRandomMat()
		{
			return materials [Random.Range (0, materials.Length)];
		}
	}

	[HideInInspector]
	public BIOMES[,] biomeMap; 

	public BiomeSet[] biomeInfo;

	[HideInInspector]
	public IndexStack<Vec3Int>[] biomeOccupiedChunks;



	int num_chunks_x;
	int num_chunks_y;
	int num_chunks_z;

	// Use this for initialization
	void Awake ()
	{
		vxe = VoxelExtractionPointCloud.Instance;
		chunkObjs = vxe.chunkGameObjects;
        
		num_chunks_x = vxe.num_chunks_x;
		num_chunks_y = vxe.num_chunks_y;
		num_chunks_z = vxe.num_chunks_z;

		biomeOccupiedChunks = new IndexStack<Vec3Int>[activeBiomes];
		for(int i=0;i<activeBiomes;i++)
		{
			biomeOccupiedChunks[i] = new IndexStack<Vec3Int>(new Vec3Int[1000]);
		}
		
		initBiomes ();
	}

	void initBiomes ()
	{
		biomeMap = new BIOMES[num_chunks_x, num_chunks_z];

		for (int i=0; i<num_chunks_x; i++)
			for (int j=0; j<num_chunks_z; j++) {
				biomeMap [i, j] = BIOMES.sand;

				Vector2 myvec = new Vector2 (i, j);
				for (int k=0; k<biomeArea.Length; k++) {
					Vector2 localvec = myvec - biomeArea [k].position;

					if (localvec.sqrMagnitude < (biomeArea [k].radius * biomeArea [k].radius)) {
						biomeMap [i, j] = biomeArea [k].biome;
					}
				}
			}

		resetBiomes ();
	}

	public void addChunkToBiomeOccChunks(Vec3Int cc)
	{
		int biomeIndex = (int)biomeMap [cc.x, cc.z];

		biomeOccupiedChunks [biomeIndex].push (cc);
	}

	public BIOMES getBiomeFromCoords(Vec3Int cc)
	{
		return biomeMap [cc.x, cc.z];
	}

	public Material getBiomeMaterialFromCoords(Vec3Int cc)
	{
		return biomeInfo [(int)biomeMap [cc.x, cc.z]].getRandomMat ();
	}

	/// <summary>
	/// Resets the biomes.
	/// </summary>
	public void resetBiomes ()
	{
		//Debug.Log ("Swap Materials");
		for (int i=0; i<num_chunks_x; i++)
			for (int j=0; j<num_chunks_z; j++)
				for (int k=0; k<num_chunks_y; k++) {
					chunkObjs [i, k, j].GetComponent<MeshRenderer> ().material = biomeInfo [(int)biomeMap [i, j]].getRandomMat();
				}
		//Debug.Log ("Swap Materials DONE");
	}

	/// <summary>
	/// Sets all voxels to 1 Material.
	/// </summary>
	/// <param name="mat">Mat.</param>
	public void setAllMaterials (Material mat)
	{
		for (int i=0; i<num_chunks_x; i++)
			for (int j=0; j<num_chunks_z; j++)
				for (int k=0; k<num_chunks_y; k++) {
					chunkObjs [i, k, j].GetComponent<MeshRenderer> ().material = mat;
				}
	}

}
