using UnityEngine;
using System.Collections;

public class HandTrackingScript : Singleton<HandTrackingScript> {

	IndexStack<int> frontRenderer;
	int[] pointArr;
	const int frontPointCount = 3000;
	
	IndexStack<int> handPoints;
	int[] handPointArr;
	const int handPointCount = 1000;
	
	int debug_iter = 0;
	public Camera camera;
	public GameObject pointer;
	// Use this for initialization
	void Start () {
		pointArr = new int[frontPointCount];
		frontRenderer = new IndexStack<int> (pointArr);
		
		handPointArr = new int[frontPointCount];
		handPoints = new IndexStack<int> (handPointArr);
	}

	public void DoHandTracking( Vector3[] m_points, int m_pointsCount )
	{
		int minPoint = -1;
		float sqrMinDist = 10000;
		frontRenderer.clear();
		for (int i = 0; i < m_pointsCount; ++i)
		{
			float sqrDistToCam = (camera.transform.position - m_points[i]).sqrMagnitude; 
			
			if( sqrDistToCam < 0.5f)
			{	
				if(frontRenderer.getCount() < frontPointCount)
					frontRenderer.push (i);
				
				if( sqrDistToCam < sqrMinDist )
				{
					sqrMinDist = sqrDistToCam;
					minPoint = i;
				}
			}

		}

		if(minPoint != -1)
		{
			handPoints.clear();
			Vector3 minPointP = m_points[ minPoint ];
			bool canTrack = true;
			for (int i = 0; i < frontRenderer.getCount(); ++i)
			{
				int index = frontRenderer.peek (i);
				
				float sqrDistToCam = (minPointP - m_points[ index ]).sqrMagnitude;
				
				if(handPoints.getCount() >= handPointCount)
				{
					canTrack = false;
					break;
				}
				
				if( sqrDistToCam < 0.04f)
				{
					handPoints.push (index);
				}
				
			}
			
			canTrack &= handPoints.getCount() > 30;
			
			if(canTrack)
			{
				
				pointer.SetActive(true);
				
				Vector3 centroid = m_points[ minPoint ];
				
				for(int k=0; k<3; k++)
				{
					Vector3 nextCentroid = Vector3.zero;
					int groupCount =  0;
					for(int i=0;i<handPoints.getCount();i++)
					{
						Vector3 p = m_points[ handPoints.peek(i) ];
						float sqrDist = (p - centroid).sqrMagnitude;
						
						if(sqrDist < 0.04)
						{
							nextCentroid += p;
							groupCount++;
						}
					}
					
					nextCentroid /= groupCount;
					debug_iter = k;
					if( (centroid - nextCentroid).sqrMagnitude < 0.0004 )
						break;
					
					centroid = nextCentroid;
				}
				
				pointer.transform.position = centroid;
			}
			else
			{
				pointer.SetActive(false);
			}
		}
		else
		{
			pointer.SetActive(false);
		}
	}
	

}
