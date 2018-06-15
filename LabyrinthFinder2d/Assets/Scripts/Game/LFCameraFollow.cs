using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFCameraFollow : MonoBehaviour {

	public Vector3 leftBottomCorner;
	public Vector3 topRightCorner;
//	public float maxX = 4.4f;
//	public float minX = -4.15f;
//	public float maxY = 6.6f;
//	public float minY = -6.6f;
//	public float maxZ = -11.0f;
//	public float minZ = -9.0f;
	public float dampTime = 0.15f;
	public Transform target;

	private Vector3 velocity = Vector3.zero;

	void Update () 
	{
		if (target)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			destination = new Vector3(
				Mathf.Clamp(destination.x, leftBottomCorner.x, topRightCorner.x),
				Mathf.Clamp(destination.y, leftBottomCorner.y, topRightCorner.y),
				Mathf.Clamp(destination.z, leftBottomCorner.z, topRightCorner.z));
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

	}

	void OnDrawGizmos()
	{
		
	}
}
