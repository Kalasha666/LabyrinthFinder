using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFLabyrinthNode {

	private bool _isWall;
	private Vector3 _worldPosition;
	private int _gridX;
	private int _gridY;

	public LFLabyrinthNode(bool isWall, Vector3 worldPos, int gridX, int gridY)
	{
		_isWall = isWall;
		_worldPosition = worldPos;
		_gridX = gridX;
		_gridY = gridY;
	}

	public bool IsWall
	{
		get{return _isWall;}
		set{_isWall = value;}
	}

	public Vector3 WorldPosition
	{
		get{return _worldPosition;}
		set{_worldPosition = value;}
	}

	public int GridX
	{
		get{ return _gridX;}
		set{ _gridX = value;}
	}

	public int GridY
	{
		get{ return _gridY;}
		set{ _gridY = value;}
	}
}
