using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreScanScript : MonoBehaviour
{
	public Camera leftCam, rightCam, backCam;	
	public AudioSource au_source;
	public Canvas canvas;
	public Text buttonText, textUI;
	public string[] scanMsgs;
	bool VRmode = false;
	bool doneWithMessage = false;
	VoxelExtractionPointCloud vxe;
	int requiredChunkCount = 150;
	float maxTime = 12f;	
	float timer = 0f;
	int chunkCounts = 0, prevChunkCount = 0, instructionCount = -1;
	int allMask, noMask;
	Animator myAnim;

	// Use this for initialization
	void Start ()
	{
		myAnim = GetComponent<Animator> ();
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
			canvas.worldCamera = backCam;
			//canvas [1].gameObject.SetActive (false);
		}

		vxe = VoxelExtractionPointCloud.Instance;
		//StartCoroutine (runitPreScanMessage ());

	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;
		if (!doneWithMessage && timer > 10f) {
			myAnim.SetBool ("Play", true);
			//chunkCounts = vxe.occupiedChunks.getCount ();
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

		textUI.text = scanMsgs [instructionCount];
		//if (VRmode)
		//	textUI [1].text = scanMsgs [instructionCount];
		au_source.Play ();

		if (instructionCount == scanMsgs.Length - 1)
			buttonText.text = "START";

	}

	public void pressForMessage ()
	{	
		doneWithMessage = instructionCount + 1 >= 3;
		
		if (doneWithMessage) {
			DoneScanning ();
			au_source.Play ();
		}
		
		UpdatePreScanMessage ();

		myAnim.StopPlayback ();
		myAnim.SetBool ("Play", false);
		timer = 0f;
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
		canvas.gameObject.SetActive (false);
		if (!VRmode) {
			leftCam.gameObject.SetActive (true);
			rightCam.gameObject.SetActive (true);
			backCam.clearFlags = CameraClearFlags.SolidColor;
			backCam.cullingMask = noMask;
			backCam.GetComponent<AudioListener> ().enabled = false;
		}
		//else 
		//	canvas [1].gameObject.SetActive (false);
		//otherimage.gameObject.SetActive (false);
		//otherButton.gameObject.SetActive (false);
		
		//this.enabled = false;
	}

	public void ForceDoneScanning ()
	{
		doneWithMessage = true;
		DoneScanning ();
	}
	/*IEnumerator runitPreScanMessage ()
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
		textUI.text = "You may put on the headset";
		//if (VRmode)
		//	textUI [1].text = "You may put on the headset";
		au_source.Play ();
		//yield return new WaitForSeconds (5f);
		//textUI [0].text = " ";
		//if (VRmode)
		//	textUI [1].text = " ";
		//DoneScanning ();
	}*/


	/*
	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag (compareTag)) {
			canvas [0].gameObject.SetActive (true);

			handMesh.enabled = true;
			handUIGameObject.enabled = true;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.CompareTag (compareTag)) {
			canvas [0].gameObject.SetActive (true);

			handMesh.enabled = false;
			handUIGameObject.enabled = false;
		}
	}*/


}
