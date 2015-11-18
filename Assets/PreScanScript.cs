using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreScanScript : MonoBehaviour
{

	public Camera leftCam, rightCam, backCam;
	public GameObject canvas;
	public Text textUI;
	public AudioSource au_source;
	public string[] scanMsgs;
	public float maxTime = 10f;
	public int voxelCount = 200;
	public bool triggered = false;
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
		backCam.cullingMask = allMask;
		leftCam.gameObject.SetActive (false);
		rightCam.gameObject.SetActive (false);


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
		if (instructionCount + 1 >= scanMsgs.Length - 1)
			return;

		instructionCount++;
		textUI.text = instructionCount + " " + scanMsgs [instructionCount];
		au_source.Play ();
	}

	IEnumerator runitPreScanMessage ()
	{
		yield return new WaitForSeconds (5f);
		UpdatePreScanMessage ();
		while (!triggered) {
			if (chunkCounts - prevChunkCount > 40f && timer > 7.5f) {
				timer = 0f;
				UpdatePreScanMessage ();
				prevChunkCount = chunkCounts;
			}
			yield return null;
		}

		textUI.text = scanMsgs [scanMsgs.Length - 1];
		au_source.Play ();
		yield return new WaitForSeconds (3f);
		textUI.text = " ";
		DoneScanning ();
	}

	void DoneScanning ()
	{
		//leftCam.cullingMask = allMask;
		//rightCam.cullingMask = allMask;
		canvas.SetActive (false);
		leftCam.gameObject.SetActive (true);
		rightCam.gameObject.SetActive (true);
		backCam.clearFlags = CameraClearFlags.SolidColor;
		backCam.cullingMask = noMask;

	}
}
