using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreScanScript : MonoBehaviour
{

	public Camera leftCam, rightCam, backCam;
	public GameObject[] canvas;
	public Text[] textUI;
	public AudioSource au_source;
	public string[] scanMsgs;
	public float maxTime = 10f;
	public int voxelCount = 200;
	public bool VRmode = false;
	bool triggered = false;
	VoxelExtractionPointCloud vxe;

	float timer = 0f;
	int chunkCounts = 0, prevChunkCount = 0, instructionCount = -1;
	int allMask, noMask;

	// Use this for initialization
	void Start ()
	{
		allMask = leftCam.cullingMask;
		noMask = backCam.cullingMask;

		//leftCam.cullingMask = noMask;
		//rightCam.cullingMask = noMask;
		if (!VRmode) {
			backCam.cullingMask = allMask;
			backCam.clearFlags = CameraClearFlags.Skybox;
			backCam.GetComponent<AudioListener> ().enabled = true;
			leftCam.gameObject.SetActive (false);
			rightCam.gameObject.SetActive (false);
			canvas [0].GetComponent<Canvas> ().worldCamera = backCam;
			canvas [1].SetActive (false);
		}

		vxe = VoxelExtractionPointCloud.Instance;

		StartCoroutine (runitPreScanMessage ());

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (!triggered) {
			timer += Time.deltaTime;
			chunkCounts = vxe.occupiedChunks.getCount ();
			triggered = chunkCounts > voxelCount;
		}
		//textMesh.text = "Time " + time + "\n\nChunk Count " + chunkCounts;
	}

	/// <summary>
	/// Updates the pre scan instructions.
	/// </summary>
	void UpdatePreScanMessage ()
	{
		if (instructionCount + 1 >= scanMsgs.Length)
			return;

		instructionCount++;
		textUI [0].text = scanMsgs [instructionCount];
		if (VRmode)
			textUI [1].text = scanMsgs [instructionCount];
		au_source.Play ();
	}

	IEnumerator runitPreScanMessage ()
	{
		yield return new WaitForSeconds (5f);
		UpdatePreScanMessage ();
		timer = 0f;

		while (!triggered) {
			if (chunkCounts - prevChunkCount > 40f && timer > 6f) {
				timer = 0f;
				UpdatePreScanMessage ();
				prevChunkCount = chunkCounts;
			}
			yield return null;
		}
		textUI [0].text = "Have Fun";
		if (VRmode)
			textUI [1].text = "Have Fun";
		au_source.Play ();
		yield return new WaitForSeconds (3f);
		textUI [0].text = " ";
		if (VRmode)
			textUI [1].text = " ";
		DoneScanning ();
	}

	void DoneScanning ()
	{
		//leftCam.cullingMask = allMask;
		//rightCam.cullingMask = allMask;
		canvas [0].SetActive (false);
		if (!VRmode) {
			leftCam.gameObject.SetActive (true);
			rightCam.gameObject.SetActive (true);
			backCam.clearFlags = CameraClearFlags.SolidColor;
			backCam.cullingMask = noMask;
			backCam.GetComponent<AudioListener> ().enabled = false;
		} else 
			canvas [1].SetActive (false);
	}
}
