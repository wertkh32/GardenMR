using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BIOMES : int
{
	grass = 0,
	ice = 1,
	water = 2,
	sand = 3,
}

public class BiomeScript : Singleton<BiomeScript>
{
	VoxelExtractionPointCloud vxe;
	GameObject[,,] chunkObjs;

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
		public Material tranformMat;
		public BIOMES biome;
	}

	[HideInInspector]
	public BIOMES[,]
		biomeMap; 

	public Material[] materials, fadedMaterials;

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

	public BIOMES getBiomeFromCoords(Vec3Int cc)
	{
		return biomeMap [cc.x, cc.z];
	}

	public Material getBiomeMaterialFromCoords(Vec3Int cc)
	{
		return materials [(int)biomeMap [cc.x, cc.z]];
	}

	/// <summary>
	/// Gets the biome based on material.
	/// </summary>
	/// <returns>The biome on material.</returns>
	/// <param name="chunkCoord">Chunk coordinate.</param>
	public BIOMES getBiomeFromMaterial (Vec3Int chunkCoord)
	{
		Material mat = chunkObjs [chunkCoord.x, chunkCoord.y, chunkCoord.z].GetComponent<MeshRenderer> ().material;

		for (int i=0; i<materials.Length; i++) {
			if (mat == materials [i])
				return (BIOMES)i;
		}

		return biomeMap [chunkCoord.x, chunkCoord.z];
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
					chunkObjs [i, k, j].GetComponent<MeshRenderer> ().material = materials [(int)biomeMap [i, j]];
				}
		//Debug.Log ("Swap Materials DONE");
	}

	public void doRandomChange (int hx, int hz)
	{
		StartCoroutine (resetBiomesThread (hx, hz));
	}

	/// <summary>
	/// Resets the biomes.
	/// </summary>
	public IEnumerator resetBiomesThread (int hx, int hz, int index=-1)
	{
		//int hx = vxe.num_chunks_x / 2;
		//int hz = vxe.num_chunks_z / 2; 
		//Debug.Log ("Reset Biome Thread");
		Material randommat;
		if (index > -1 && index < materials.Length)
			randommat = materials [index];
		else
			randommat = materials [Random.Range (0, 4)];

		for (float r = 0; r < 30; r+=0.5f) {
			int counter = 0;
			for (int i=0; i<vxe.occupiedChunks.getCount(); i++) {
				Vec3Int cc = vxe.occupiedChunks.peek (i);
				int x = cc.x - hx;
				int z = cc.z - hz;
				float sqrlen = ((x * x) + (z * z));
				if (sqrlen >= r * r && sqrlen < (r + 0.5f) * (r + 0.5f)) {
					chunkObjs [cc.x, cc.y, cc.z].GetComponent<MeshRenderer> ().material = randommat;
					//Debug.Log ("changed" + cc.x +  " " + cc.y + " " + cc.z);
					counter++;

					if (counter % Mathf.FloorToInt (r / 3.0f * r / 3.0f + 1.0f) == 0)
						yield return new WaitForSeconds (0.06f);
						
				}


			}


		}
		/*//Debug.Log ("Reset Biome Thread DONE");
		//a Safety Measure so the materials are set
		//even after more materials generate
		//Sorry for the Bad Code KH, will fix later
		setAllMaterials (randommat);*/
	}
	/*
	void Update()
	{

		if(changeTex)
		{
			changeTex = false;
			StartCoroutine(resetBiomesThread());
		}
	}*/

	/// <summary>
	/// Swaps the materials for the biome with NewMat, and then resets all the biomes materials
	/// </summary>
	/// <param name="newMat">New materials to use.</param>
	public void swapMaterials (ref Material[] newMat, bool reset =true)
	{
		Material[] tempMat = materials;
		materials = newMat;
		newMat = tempMat;

		//setAllMaterials(materials[0]);
		if (reset)
			resetBiomes ();
	}

	public void swapMaterialsThread (ref Material[] newMat, int hx, int hz, int index=-1)
	{
		Material[] tempMat = materials;
		materials = newMat;
		newMat = tempMat;
		
		//setAllMaterials(materials[0]);
		StartCoroutine (resetBiomesThread (hx, hz, index));
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
