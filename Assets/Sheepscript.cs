using UnityEngine;
using System.Collections;

public class Sheepscript : VoxelParent {
	public GameObject sheepModel;
	public SimpleAnimationController ani;

	protected override void Awake ()
	{
		sheepModel.SetActive (false);
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
	
	protected override void allTriggeredEvent ()
	{
		base.allTriggeredEvent ();
		sheepModel.SetActive (true);
		ani.eventfunc = startAI;
		ani.StartAnimation ();
		ItemSpawner.Instance.canSpawn = true;
		
	}

	public void startAI()
	{
		sheepModel.GetComponent<JumpingAI> ().init ();
	}
	
	public override void voxelSwitchEvent ()
	{
		base.voxelSwitchEvent ();
		
	}
}
