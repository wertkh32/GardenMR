using UnityEngine;
using System.Collections;



public class PetManager : Singleton<PetManager>
{
	/*
	enum MessageState
	{
		TUTORIAL = 0,
		ISCLOSE = 1,
		THANKYOU = 2,
		SLEEPING = 3,
		NOTHING = 4
	}

    public const float SNIFF_RATE = 2.0f;

	public Texture2D tutorial;
	public Texture2D thankyou;
	public Texture2D isclose;
	public Texture2D sleeping;
	public AudioSource audioSource;
    public AudioClip isIdleAudio;
	public AudioClip isCloseAudio;
	public AudioClip thankYouAudio;

    private float nextSniffAudioTime = 0;
	ItemSpawner itemspawn;
	VoxelExtractionPointCloud vxe;
	public Camera camera;
	MessageState state;
	// Use this for initialization
	void Start ()
	{
		itemspawn = ItemSpawner.Instance;
		vxe = VoxelExtractionPointCloud.Instance;
		state = MessageState.NOTHING;
		GetComponent<JumpingAI> ().init ();
		StartCoroutine (messageSystem ());

        nextSniffAudioTime = Time.time + SNIFF_RATE;
	}

	IEnumerator messageSystem ()
	{
		XZBillboard.Instance.changeTexture (tutorial);
		state = MessageState.TUTORIAL;
		yield return new WaitForSeconds (10.0f);

		XZBillboard.Instance.hide ();

		while (true) {
			if (state != MessageState.ISCLOSE) {
				for (int i=0; i<itemspawn.currentItemToSpawn; i++) {					
					if (itemspawn.spawneditems [i] == null || itemspawn.spawneditems [i].GetComponent<TriggerScript> ().triggered)
						continue;

					float groundLength = (itemspawn.spawneditems [i].transform.position - transform.position).magnitude;
					if (groundLength < vxe.voxel_size * 25) {
						XZBillboard.Instance.show ();
						state = MessageState.ISCLOSE;
						if (!audioSource.isPlaying) {
                            audioSource.clip = isCloseAudio;
							audioSource.Play ();
						}
						XZBillboard.Instance.changeTexture (isclose);
						yield return new WaitForSeconds (10.0f);
						XZBillboard.Instance.hide ();
						state = MessageState.NOTHING;
						audioSource.Stop ();
						break;
					}
				}
			}

			if (state == MessageState.THANKYOU) {
				//audioSource.clip = happyRobot;
                audioSource.Stop();
				audioSource.PlayOneShot (thankYouAudio);
				yield return new WaitForSeconds (5.0f);
				XZBillboard.Instance.hide ();
				state = MessageState.NOTHING;
			}

			yield return new WaitForSeconds (1.0f);
		}
	}

	public void setThankYou ()
	{
		state = MessageState.THANKYOU;
		XZBillboard.Instance.show ();
		XZBillboard.Instance.changeTexture (thankyou);
	}

	// Update is called once per frame
	void Update ()
	{
	    if (Time.time > nextSniffAudioTime)
        {
            audioSource.PlayOneShot(isIdleAudio);
            nextSniffAudioTime = Time.time + Random.Range(SNIFF_RATE, SNIFF_RATE*5);
        }
	}
	*/
}
