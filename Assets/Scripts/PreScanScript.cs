using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreScanScript : MonoBehaviour
{
	public Camera leftCam, rightCam, backCam;

	public Canvas canvas;
	public Text textUI;
	public GameObject headsetImage;
	public GameObject instructionImage;
	public string[] scanMsgs;
	public bool VRmode = true;
	bool doneWithMessage = false;
	VoxelExtractionPointCloud vxe;
	Animator myAnim;
	float timer = 0f;
	int chunkCounts = 0, prevChunkCount = 0, instructionCount = 0;
	int allMask, noMask;

	// Use this for initialization
	void Start ()
	{
		allMask = leftCam.cullingMask;
		noMask = backCam.cullingMask;

		backCam.cullingMask = allMask;
		backCam.clearFlags = CameraClearFlags.Skybox;
		backCam.GetComponent<AudioListener> ().enabled = true;
		leftCam.gameObject.SetActive (false);
		rightCam.gameObject.SetActive (false);
		canvas.worldCamera = backCam;

		headsetImage.SetActive (false);
		vxe = VoxelExtractionPointCloud.Instance;
		UpdatePreScanMessage ();
	}

	/// <summary>
	/// Updates the pre scan instructions.
	/// </summary>
	void UpdatePreScanMessage ()
	{
		textUI.text = scanMsgs [instructionCount];
		instructionCount++;
	}

	public void pressForMessage ()
	{	
		doneWithMessage = instructionCount >= scanMsgs.Length;

		if (doneWithMessage) 
		{
			instructionImage.SetActive(false);
			headsetImage.SetActive (true);
		}
		else
		{
			UpdatePreScanMessage ();
		}

	}

	public void DoneScanning ()
	{
		//leftCam.cullingMask = allMask;
		//rightCam.cullingMask = allMask;
		canvas.gameObject.SetActive (false);

		if (VRmode) {
			leftCam.gameObject.SetActive (true);
			rightCam.gameObject.SetActive (true);
			backCam.clearFlags = CameraClearFlags.SolidColor;
			backCam.cullingMask = noMask;
			backCam.GetComponent<AudioListener> ().enabled = false;
		}

		ItemSpawner.Instance.startSpawining ();
		this.enabled = false;

	}

	public void ForceDoneScanning ()
	{
		doneWithMessage = true;
		DoneScanning ();
	}
}
