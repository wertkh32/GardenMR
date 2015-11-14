using UnityEngine;
using System.Collections;

public class PreScanScript : MonoBehaviour
{

	public Camera leftCam, rightCam, backCam;
	public TextMesh textMesh;
	public float maxTime = 10f;
	public int voxelCount = 200;
	public bool triggered = false;
	VoxelExtractionPointCloud vxe;


	float time = 0f;
	int chunkCounts = 0;
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


	}
	
	// Update is called once per frame
	void Update ()
	{

		if (!triggered) {
			time += Time.deltaTime;
			chunkCounts = vxe.occupiedChunks.getCount ();
			triggered = chunkCounts > voxelCount;
		}
		textMesh.text = "Time " + time + "\n\nChunk Count " + chunkCounts;
	}

	void DoneScanning ()
	{
		//leftCam.cullingMask = allMask;
		//rightCam.cullingMask = allMask;

		leftCam.gameObject.SetActive (true);
		rightCam.gameObject.SetActive (true);

		backCam.cullingMask = noMask;

	}
}
