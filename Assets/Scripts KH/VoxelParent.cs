using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelParent : MonoBehaviour
{
	
	public Camera camera;
	public ParticleSystem partsys;
	public Transform voxelWireFramesTrans;
	public VoxelSwitch[] switches;
	public int stage;
	int num_switch;
	public int num_triggered;
	public bool allTriggered = false;
	public Vec3Int chunkCoords;

	bool forcefield = true;

	Material origMat;
	protected AudioSource audioSource;


	// Use this for initialization
	protected VoxelExtractionPointCloud vxe;
	protected BoxCollider mycollider;

	int up_credits = 10;

	protected virtual void Awake ()
	{

	}

	protected virtual void Start ()
	{
		mycollider = GetComponent<BoxCollider> ();
		audioSource = GetComponent<AudioSource> ();

		vxe = VoxelExtractionPointCloud.Instance;
		partsys.enableEmission = true;

		if (camera == null)
			camera = vxe.camera;

		if (partsys.isPlaying) 
			partsys.Stop ();

		if (switches.Length < 1)
			InitSwitches ();

		num_switch = switches.Length;
		num_triggered = 0;

		for (int i=0; i< num_switch; i++) {
			switches [i].vparent = this;
		}
	
		audioSource.clip = AudioManager.Instance.spawnClip;
		audioSource.Play ();

	}

	void InitSwitches ()
	{
		List<VoxelSwitch> vswitchList = new List<VoxelSwitch> ();
		VoxelSwitch vswitch;

		if (voxelWireFramesTrans == null)
			voxelWireFramesTrans = transform.FindChild ("VoxelWireframes");

		for (int i=0; i<voxelWireFramesTrans.childCount; i++) {
			//if (!childList [i].CompareTag (biome.tag)) {
			vswitch = null;
			vswitch = voxelWireFramesTrans.GetChild (i).GetComponent<VoxelSwitch> ();
			if (vswitch != null)
				vswitchList.Add (vswitch);

		}
		
		switches = vswitchList.ToArray ();
		//Debug.Log (this.name + " switch length " + switches.Length);

	}

	/// <summary>
	/// Checks for voxels in collider dir.
	/// </summary>
	/// <returns><c>true</c>, if for voxels in collider dir was checked, <c>false</c> otherwise.</returns>
	/// <param name="pushdir">Pushdir.</param>
	bool checkForVoxelsInColliderDir (ref Vector3 pushdir)
	{
		Vector3 center = mycollider.bounds.center;
		Vector3 max = center + mycollider.bounds.extents;
		Vector3 min = center - mycollider.bounds.extents;
		
		for (float i=min.x; i<=max.x; i+= vxe.voxel_size)
			for (float j=min.y; j<=max.y; j+= vxe.voxel_size)
				for (float k=min.z; k<=max.z; k+= vxe.voxel_size) {
					if (vxe.isVoxelThere (new Vector3 (i, j, k))) {
						Vector3 dir = Vector3.zero;
					
						if (i < center.x) {
							pushdir += Vector3.right;
						} else {
							pushdir += Vector3.left;
						}
					
						if (k < center.z) {
							pushdir += Vector3.forward;
						} else {
							pushdir += Vector3.back;
						}

						if(up_credits > 0)
						{
							pushdir += Vector3.up;
							up_credits--;
						}
						return true;
					}
				}
		
		return false;
	}

	protected virtual void playerCloseEvent()
	{
		if (!partsys.isPlaying)
		{
			partsys.Play ();
		}
	}
	
	protected virtual void playerFarEvent ()
	{
		if (partsys.isPlaying)
		{
			partsys.Stop ();
		}
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		if (!allTriggered && num_triggered == num_switch) {
			allTriggered = true;
			allTriggeredEvent ();
		}

		if (!allTriggered) {
			float dist = Vector3.ProjectOnPlane ((camera.transform.position - transform.position), Vector3.up).magnitude;
			if (dist < 8 * vxe.voxel_size)
			{
				forcefield = false;
				playerCloseEvent();

			} 
			else 
			{
				forcefield = true;
				playerFarEvent();
			}

			Vector3 dir = Vector3.zero;
			if (forcefield) 
			{
				if (checkForVoxelsInColliderDir (ref dir)) 
				{
					transform.position += dir * vxe.voxel_size;
				}
			}
		}


	}



	protected virtual void  allTriggeredEvent ()
	{
		//partsys.gameObject.transform.position = transform.position;
		partsys.startLifetime = 1;
		partsys.startColor = Color.green;
		partsys.startSpeed = 2.0f;
		partsys.startSize = 0.1f;
		partsys.maxParticles = 500;
		partsys.Clear ();
		partsys.Stop ();
		partsys.Emit (500);

		for (int i=0; i<num_switch; i++) 
		{
			switches [i].gameObject.SetActive (false);
		}

	}

	public virtual void voxelSwitchEvent ()
	{
		audioSource.PlayOneShot (AudioManager.Instance.doReMi[num_triggered % AudioManager.Instance.doReMiNum]);
		num_triggered++;
	}

}
