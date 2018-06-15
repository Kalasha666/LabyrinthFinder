using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFGridNode{

	private bool _isCanWalk;
	private Vector3 _worldPosition;
	private int _gCost;
	private int _hCost;
	private int _gridX;
	private int _gridY;
	private float _size;
	private LFGridNode _parentNode;

	public LFGridNode(bool isCanWalk, Vector3 worldPos, int gridX, int gridY, float size)
	{
		_isCanWalk = isCanWalk;
		_worldPosition = worldPos;
		_gridX = gridX;
		_gridY = gridY;
		_size = size;
	}

	public bool IsCanWalk
	{
		get{return _isCanWalk;}
		set{_isCanWalk = value;}
	}

	public Vector3 WorldPosition
	{
		get{return _worldPosition;}
		set{_worldPosition = value;}
	}

	public int GCost
	{
		get{ return _gCost;}
		set{ _gCost = value; }
	}

	public int HCost
	{
		get{ return _hCost;}
		set{ _hCost = value;}
	}

	public int FCost
	{
		get{ return _gCost + _hCost;}
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

	public float Size
	{
		get{ return _size;}
		set{ _size = value;}
	}

	public LFGridNode ParentNode
	{
		get{ return _parentNode;}
		set{ _parentNode = value;}
	}
}
