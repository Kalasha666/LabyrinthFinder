using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFGrid : MonoBehaviour {

	public LayerMask wallMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public bool isDrawGizmos = false;

	private LFGridNode[,] _grid;
	private float _nodeDiameter;
	private int _gridSizeX;
	private int _gridSizeY;

	// Use this for initialization
	void Start () {

	}

	public void StartInitGrid()
	{
		_nodeDiameter = nodeRadius * 2;
		_gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
		_gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
		InitGrid();
	}

	void OnDrawGizmos()
	{
		if(!isDrawGizmos) return;

		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

		if(_grid != null)
		{
			foreach(LFGridNode node in _grid)
			{
				Gizmos.color = (node.IsCanWalk)? Color.green: Color.red;
				Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * _nodeDiameter);
			}
		}

	}

	public LFGridNode GridNodeFromWorldPosition(Vector3 worldPosition)
	{
		float perX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float perY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
		perX = Mathf.Clamp01(perX);
		perY = Mathf.Clamp01(perY);
		int x = Mathf.RoundToInt((_gridSizeX - 1) * perX);
		int y = Mathf.RoundToInt((_gridSizeY - 1) * perY);

		return _grid[x, y];
	}

	public List<LFGridNode> GetNeighbours(LFGridNode node)
	{
		List<LFGridNode> neighbours = new List<LFGridNode> ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if ((x == 0 && y == 0) || (x == 1 && y == 1) || (x == -1 && y == -1) || (x == -1 && y == 1) || (x == 1 && y == -1))
					continue;

				int checkX = node.GridX + x;
				int checkY = node.GridY + y;

				if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY) {
					neighbours.Add (_grid [checkX, checkY]);
				}
			}
		}

		return neighbours;
	}

	private void InitGrid()
	{
		_grid = new LFGridNode[_gridSizeX, _gridSizeY];
		Vector3 leftBottomCorner = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

		for (int x = 0; x < _gridSizeX; x++)
		{
			for(int y = 0; y < _gridSizeY; y++)
			{
				Vector3 worldPoint = leftBottomCorner + Vector3.right * (x * _nodeDiameter + nodeRadius) + Vector3.up * (y * _nodeDiameter + nodeRadius);
				bool isCanWalk = !(Physics.CheckSphere(worldPoint, nodeRadius, wallMask));
				_grid[x, y] = new LFGridNode(isCanWalk, worldPoint, x, y, _nodeDiameter);
			}
		}
	}


}
