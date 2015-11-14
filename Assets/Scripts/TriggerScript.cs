using UnityEngine;
using System.Collections;

public class TriggerScript : MonoBehaviour
{

	public bool triggered = false;
	public bool debug = false;
	//public bool init = false;
	public Camera camera;
	public ParticleSystem partsys;
	public GameObject littleSheep;
	public GameObject bush;
	public Material noglowmat;
	public Material glowmat;
	private bool isSleeping = true;


	Vector3[] fourpts;
	VoxelExtractionPointCloud vxe;

	CameraClearFlags defaultFlag;
	float defaultLightIntensity;
	BoxCollider mycollider;
	// Use this for initialization
	void Awake ()
	{
		mycollider = GetComponent<BoxCollider> ();
	}

	void Start ()
	{
		vxe = VoxelExtractionPointCloud.Instance;
		partsys.Stop ();
		//cubeswitch.gameObject.SetActive (false);
	}

	bool checkForVoxelsInCollider ()
	{
		Vector3 center = mycollider.bounds.center;
		Vector3 max = center + mycollider.bounds.extents;
		Vector3 min = center - mycollider.bounds.extents;
		
		for (float i=min.x; i<=max.x; i+= vxe.voxel_size)
			for (float j=min.y; j<=max.y; j+= vxe.voxel_size)
			for (float k=min.z; k<=max.z; k+= vxe.voxel_size) {
				if (vxe.isVoxelThere (new Vector3 (i, j, k)))
				{
					return true;
				}
			}
		
		return false;
	}


	bool checkForVoxelsInColliderDir (ref Vector3 pushdir)
	{
		Vector3 center = mycollider.bounds.center;
		Vector3 max = center + mycollider.bounds.extents;
		Vector3 min = center - mycollider.bounds.extents;

		for (float i=min.x; i<=max.x; i+= vxe.voxel_size)
			for (float j=min.y; j<=max.y; j+= vxe.voxel_size)
				for (float k=min.z; k<=max.z; k+= vxe.voxel_size) {
					if (vxe.isVoxelThere (new Vector3 (i, j, k)))
					{
						Vector3 dir = Vector3.zero;
						
						if(i < center.x)
						{
							pushdir += Vector3.right;
						}
						else
						{
							pushdir += Vector3.left;
						}

						if(k < center.z)
						{
							pushdir += Vector3.forward;
						}
						else
						{
							pushdir += Vector3.back;
						}
						
						pushdir += Vector3.up;

						return true;
					}
				}
		
		return false;
	}


	// Update is called once per frame
	void Update ()
	{
		if (!isSleeping) {
			if (!triggered && checkForVoxelsInCollider ()) {
				triggeredEvent ();
			}
		}

		Vector3 dir = Vector3.zero;
		if (!triggered) 
		{
			if (isSleeping && 
				(vxe.isVoxelThere (littleSheep.transform.position) || checkForVoxelsInColliderDir (ref dir)) 
		    ) {
				littleSheep.transform.position += dir * vxe.voxel_size;
				transform.position += dir * vxe.voxel_size;
			}

			float dist = Vector3.ProjectOnPlane ((camera.transform.position - transform.position), Vector3.up).magnitude;
			if (dist < 10 * vxe.voxel_size) {
				//triggeredEvent();
				//partsys.Emit (100);
				if (isSleeping) {
					isSleeping = false;
					partsys.Play ();
				}
			} else {
				isSleeping = true;
				partsys.Stop ();
			}
		}
		//if(!triggered)
		//{
			//glowmat.SetFloat("_floatTime",Mathf.Sin(Time.time * 2));
		//}
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (triggered)
			return;

	}

	void triggeredEvent ()
	{
		partsys.startLifetime = 3;
		partsys.startColor = Color.white;
		partsys.startSpeed = 2.0f;
		partsys.startSize = 0.3f;
		partsys.maxParticles = 500;
		partsys.Clear ();
		partsys.Stop ();
		partsys.Emit (500);

		littleSheep.GetComponent<JumpingAI> ().init ();
		littleSheep.GetComponent<AudioSource> ().Play ();
		//bush.GetComponent<MeshRenderer> ().material = noglowmat;
		PetManager.Instance.setThankYou ();

		triggered = true;

		//No Random Change right now...
		//Vec3Int cc = vxe.getChunkCoords (transform.position);
		//BiomeScript.Instance.doRandomChange (cc.x, cc.z);
		ItemSpawner.Instance.canSpawn = true;
	}

}
