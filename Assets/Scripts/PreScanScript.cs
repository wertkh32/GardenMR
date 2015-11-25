using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreScanScript : MonoBehaviour
{
	public TutorialGaze tutorialGazeScript;
	public Camera leftCam, rightCam, backCam;	
	public AudioSource au_source;
	public int requiredChunkCount = 200;
	public float maxTime = 9f;	
	public Text buttonText;
	public Canvas[] canvas;
	public Text[] textUI;
	public string[] scanMsgs;
	bool VRmode = false;
	bool doneWithMessage = false;
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
			canvas [0].worldCamera = backCam;
			//canvas [1].gameObject.SetActive (false);
		}

		vxe = VoxelExtractionPointCloud.Instance;
		tutorialGazeScript.enabled = false;
		//StartCoroutine (runitPreScanMessage ());

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (!doneWithMessage) {
			timer += Time.deltaTime;
			chunkCounts = vxe.occupiedChunks.getCount ();
		}
		//textMesh.text = "Time " + time + "\n\nChunk Count " + chunkCounts;
	}

	/// <summary>
	/// Updates the pre scan instructions.
	/// </summary>
	void UpdatePreScanMessage ()
	{
		if (doneWithMessage) {
			return;
		}

		instructionCount++;

		textUI [0].text = scanMsgs [instructionCount];
		if (VRmode)
			textUI [1].text = scanMsgs [instructionCount];
		au_source.Play ();

		if (instructionCount == scanMsgs.Length - 1)
			buttonText.text = "START";

	}

	IEnumerator runitPreScanMessage ()
	{
		yield return new WaitForSeconds (5f);
		UpdatePreScanMessage ();
		timer = 0f;

		int scanAmount = requiredChunkCount / scanMsgs.Length;

		while (!doneWithMessage) {
			if ((chunkCounts - prevChunkCount > scanAmount && timer > 6f) || (timer > maxTime)) {
				timer = 0f;
				UpdatePreScanMessage ();
				prevChunkCount = chunkCounts;
			}
			yield return null;
		}
		textUI [0].text = "You may put on the headset";
		if (VRmode)
			textUI [1].text = "You may put on the headset";
		au_source.Play ();
		/*yield return new WaitForSeconds (5f);
		textUI [0].text = " ";
		if (VRmode)
			textUI [1].text = " ";
		DoneScanning ();*/
	}

	public void pressForMessage ()
	{	
		doneWithMessage = instructionCount + 1 >= scanMsgs.Length;

		if (doneWithMessage) {
			DoneScanning ();
			au_source.Play ();
		}

		UpdatePreScanMessage ();
		/*yield return new WaitForSeconds (5f);
		textUI [0].text = " ";
		if (VRmode)
			textUI [1].text = " ";
		DoneScanning ();*/
	}

	void DoneScanning ()
	{
		//leftCam.cullingMask = allMask;
		//rightCam.cullingMask = allMask;
		canvas [0].gameObject.SetActive (false);
		if (!VRmode) {
			leftCam.gameObject.SetActive (true);
			rightCam.gameObject.SetActive (true);
			backCam.clearFlags = CameraClearFlags.SolidColor;
			backCam.cullingMask = noMask;
			backCam.GetComponent<AudioListener> ().enabled = false;
		}
		//else 
		//	canvas [1].gameObject.SetActive (false);
		this.enabled = false;
	}

	void StartGaze ()
	{
		tutorialGazeScript.enabled = true;
		tutorialGazeScript.StartGaze ();
	}

	IEnumerator waitForGaze ()
	{
		yield return null;
	}

	public void ForceDoneScanning ()
	{
		doneWithMessage = true;
		DoneScanning ();
	}
}
