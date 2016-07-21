using UnityEngine;
using System.Collections;

public class PerDepthFrameCallBack : Singleton<PerDepthFrameCallBack> {

	public void CallBack(Vector3[] m_points, int m_pointsCount)
	{
		VoxelExtractionPointCloud.Instance.addAndRender(m_points, m_pointsCount);
		HandTrackingScript.Instance.DoHandTracking(m_points, m_pointsCount);
	}
}
