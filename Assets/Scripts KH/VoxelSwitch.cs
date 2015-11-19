using UnityEngine;
using System.Collections;

public class VoxelSwitch : MonoBehaviour {
	
	public bool triggered = false;

	public Camera camera;
	public ParticleSystem partsys;
	private bool isSleeping = true;

	public Material outlineMaterial;
	public Material wholeMaterial;

	[HideInInspector]
	public VoxelParent vparent;

	VoxelExtractionPointCloud vxe;
	BoxCollider mycollider;
	MeshRenderer myrenderer;

	void Awake ()
	{
		mycollider = GetComponent<BoxCollider> ();
		myrenderer = GetComponent<MeshRenderer> ();
		vxe = VoxelExtractionPointCloud.Instance;
	}
	
	void Start ()
	{
		partsys.Stop ();
		myrenderer.material = outlineMaterial;
	}
	
	bool checkForVoxelsInCollider ()
	{
		Vector3 center = mycollider.bounds.center;
		Vector3 max = center + mycollider.bounds.extents;
		Vector3 min = center - mycollider.bounds.extents;
		float step = vxe.voxel_size * 0.5f;

		for (float i=min.x; i<=max.x; i+= step)
			for (float j=min.y; j<=max.y; j+= step)
			for (float k=min.z; k<=max.z; k+= step) {
				if (vxe.isVoxelThere (new Vector3 (i, j, k)))
				{
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
			float dist = Vector3.ProjectOnPlane ((camera.transform.position - transform.position), Vector3.up).magnitude;
			if (dist < 10 * vxe.voxel_size) 
			{
				if (isSleeping) 
				{
					isSleeping = false;
				}
			} 
			else 
			{
				isSleeping = true;
			}
		}
		
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (triggered)
			return;
		
	}
	
	void triggeredEvent ()
	{
		partsys.gameObject.transform.position = transform.position;
		partsys.startLifetime = 1;
		partsys.startColor = Color.white;
		partsys.startSpeed = 1.0f;
		partsys.startSize = 0.1f;
		partsys.maxParticles = 100;
		partsys.Clear ();
		partsys.Stop ();
		partsys.Emit (100);
		myrenderer.material = wholeMaterial;

		triggered = true;

		if(vparent != null)
		{
			vparent.voxelSwitchEvent();
		}
	}

}
