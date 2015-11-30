using UnityEngine;
using System.Collections;

public class BushScript : VoxelParent
{
	public GameObject bushModel;
	public Material ruinTexture;
	Animator wireframeAnimator;
	public SimpleAnimationController popController;
	public bool isTutorial;

	protected override void Awake ()
	{
		bushModel.SetActive (false);
		base.Awake ();
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		wireframeAnimator = GetComponent<Animator> ();
		//not everyone destroys the world
		if(ruinTexture != null)
		{
			vxe.changeChunkMaterial(chunkCoords, ruinTexture);
		}
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
			partsys.transform.position = transform.position;

			partsys.startSpeed = 1.0f;
			partsys.startSize = 0.3f;
			partsys.maxParticles = 500;
			partsys.startColor = new Color(0.6f,0.8f,0.2f);
			partsys.Clear ();
			partsys.Stop ();


			partsys.Emit(500);

			yield return new WaitForSeconds(0.2f);

			vxe.changeChunkMaterial (chunkCoords, BiomeScript.Instance.getBiomeMaterialFromCoords (chunkCoords));
		}
	}

	protected override void playerCloseEvent ()
	{
		base.playerCloseEvent ();
		wireframeAnimator.SetTrigger ("Stop");
	}

	protected override void playerFarEvent()
	{
		base.playerFarEvent ();
		wireframeAnimator.SetTrigger ("Play");
	}

	protected override void allTriggeredEvent ()
	{
		base.allTriggeredEvent ();
		bushModel.SetActive (true);

		if(stage >= ItemSpawner.Instance.currentStage)
			ItemSpawner.Instance.canSpawn = true;

		if (isTutorial)
			ItemSpawner.Instance.nextStage = true;

		if (popController != null) 
		{
			popController.eventfunc = popDone;
			popController.NextAnimation ();
		}
		else
		{
			popDone();
		}
		wireframeAnimator.SetTrigger ("Stop");

		audioSource.PlayOneShot (AudioManager.Instance.winClip);
	}

	public void popDone()
	{
		StartCoroutine (fall ());
	}

	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
	}
}
