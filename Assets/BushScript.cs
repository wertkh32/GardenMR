using UnityEngine;
using System.Collections;

public class BushScript : VoxelParent {
	public GameObject bushModel;

    public AudioSource audioSource;
    public AudioClip p1;
    public AudioClip p2;
    public AudioClip p3;
    public AudioClip p4;
    public AudioClip p5;
    public AudioClip p6;
    public AudioClip p7;
    public AudioClip p8;
    public AudioClip p9;
    public AudioClip p10;

	protected override void Awake ()
	{
		bushModel.SetActive (false);
		base.Awake ();
	}

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();
	}

	IEnumerator fall()
	{
		Vector3 coords = Vector3.zero, norm = Vector3.zero;
		bool hit = vxe.RayCast (transform.position, Vector3.down, 64, ref coords, ref norm, 0.5f);

		Vector3 startpos = transform.position;
		coords.x = startpos.x;
		coords.z = startpos.z;
		if(hit)
		{
			for(float i=0; i<0.5f; i+= Time.deltaTime)
			{
				transform.position = Vector3.Lerp(startpos, coords, i * 2);
				Debug.Log ("falling");
				yield return null;
			}
		}
	}

	protected override void allTriggeredEvent ()
	{
		base.allTriggeredEvent ();
		bushModel.SetActive (true);
		StartCoroutine (fall ());
		ItemSpawner.Instance.canSpawn = true;

	}

	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
        switch (num_triggered)
        {
            case 1:
                audioSource.PlayOneShot(p1);
                break;
            case 2:
                audioSource.PlayOneShot(p2);
                break;
            case 3:
                audioSource.PlayOneShot(p3);
                break;
            case 4:
                audioSource.PlayOneShot(p4);
                break;
            case 5:
                audioSource.PlayOneShot(p5);
                break;
            case 6:
                audioSource.PlayOneShot(p6);
                break;
            case 7:
                audioSource.PlayOneShot(p7);
                break;
            case 8:
                audioSource.PlayOneShot(p8);
                break;
            case 9:
                audioSource.PlayOneShot(p9);
                break;
            case 10:
                audioSource.PlayOneShot(p10);
                break;
            default:
                break;
        }
	}
}
