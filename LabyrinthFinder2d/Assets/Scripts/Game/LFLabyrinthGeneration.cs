using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFLabyrinthGeneration : MonoBehaviour {

	public GameObject ground;
	public GameObject wallBlockPrefab;
	public Vector2 gridWorldSize;
	public float nodeWidth;
	public int pathStepCount = 5;
	public int pathCount = 10;
	public bool isDrawGizmos = false;
	private LFLabyrinthNode[,] _grid;
	private int _gridSizeX;
	private int _gridSizeY;
	private List<GameObject> _wallBlocks;

	public void StartInitLabyrinth()
	{
		_gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeWidth);
		_gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeWidth);

		InitGrid();

		InitBorder();
		CreateWallBlocks();
	}

	void OnDrawGizmos()
	{
		if(!isDrawGizmos) return;

		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position, new Vector3(nodeWidth, nodeWidth, 1));

		if(_grid != null)
		{
			foreach(LFLabyrinthNode node in _grid)
			{
				Gizmos.color = (node.IsWall)? Color.red: Color.green;

				if(node.IsWall)
					Gizmos.DrawCube(node.WorldPosition, new Vector3(nodeWidth, nodeWidth, 1));
				else
					Gizmos.DrawWireCube(node.WorldPosition, new Vector3(nodeWidth, nodeWidth, 1));
			}
		}
	}

	private void InitGrid()
	{
		_grid = new LFLabyrinthNode[_gridSizeX, _gridSizeY];
		Vector3 leftBottomCorner = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

		for (int x = 0; x < _gridSizeX; x++)
		{
			for(int y = 0; y < _gridSizeY; y++)
			{
				Vector3 worldPoint = leftBottomCorner + Vector3.right * (x * nodeWidth + nodeWidth / 2) + Vector3.up * (y * nodeWidth + nodeWidth / 2);
				_grid[x, y] = new LFLabyrinthNode(false, worldPoint, x, y);
			}
		}
	}

	private void InitPath()
	{
		int startX = Random.Range(0, _gridSizeX );
		int startY = Random.Range(0, _gridSizeY );
		LFLabyrinthNode currentNode = _grid [startX, startY];
		int step = 0;

		while(step < pathStepCount)
		{
			currentNode.IsWall = false;
			List<LFLabyrinthNode> neighbours = NeighbourNodes(currentNode);
			LFLabyrinthNode newNeighbour = neighbours[0];
			List<LFLabyrinthNode> newNexNeighbours = NeighbourNodes(newNeighbour);
			int maxWallCount = NeighboursWallCount(newNexNeighbours);

			for(int i = 1; i < neighbours.Count; i++)
			{
				LFLabyrinthNode neighbour = neighbours[i];
				List<LFLabyrinthNode> nextNeighbours = NeighbourNodes(neighbour);
				int newWallCount = NeighboursWallCount(nextNeighbours);

				if(newWallCount >= maxWallCount)
				{
					newNeighbour = neighbour;
					maxWallCount = newWallCount;
				}
			}

			currentNode = newNeighbour;
			step += 1;
		}
	}

	private int NeighboursWallCount(List<LFLabyrinthNode> neighbours)
	{
		int wallCount = 0;

		for(int i = 0; i < neighbours.Count; i ++)
		{
			LFLabyrinthNode neighbour = neighbours[i];

			if(neighbour.IsWall) wallCount +=1;
		}

		return wallCount;
	}

	private List<LFLabyrinthNode> NeighbourNodes(LFLabyrinthNode node)
	{
		List<LFLabyrinthNode> neighbours = new List<LFLabyrinthNode> ();

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

	private void InitBorder()
	{
		for (int x = 0; x < _gridSizeX; x++)
		{
			for(int y = 0; y < _gridSizeY; y++)
			{
				if(x == 0 || y == 0 || x == (_gridSizeX - 1) || y == (_gridSizeY - 1))
				{
					_grid[x, y].IsWall = true;
				}
			}
		}
	}

	private void CreateWallBlocks()
	{
		_wallBlocks = new List<GameObject>();

		for (int x = 0; x < _gridSizeX; x++)
		{
			for(int y = 0; y < _gridSizeY; y++)
			{
				LFLabyrinthNode node = _grid [x, y];

				if(node.IsWall)
				{
					GameObject wall = Instantiate(wallBlockPrefab, new Vector3( node.WorldPosition.x,  node.WorldPosition.y , 0.5f), Quaternion.identity);
					wall.transform.parent = ground.transform;
					_wallBlocks.Add(wall);
				}
			}
		}
	}

	public LFLabyrinthNode GridNodeFromWorldPosition(Vector3 worldPosition)
	{
		float perX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float perY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
		perX = Mathf.Clamp01(perX);
		perY = Mathf.Clamp01(perY);
		int x = Mathf.RoundToInt((_gridSizeX - 1) * perX);
		int y = Mathf.RoundToInt((_gridSizeY - 1) * perY);

		return _grid[x, y];
	}

	public LFLabyrinthNode RandomFreeNodeWithOutPosition(Vector3 deletedPosition)
	{
		List<LFLabyrinthNode> notWallNodes = GetAllNotWallNodes();
		LFLabyrinthNode deletedNode = GridNodeFromWorldPosition(deletedPosition);

		if (notWallNodes.Contains (deletedNode))
			notWallNodes.Remove (deletedNode);

		LFLabyrinthNode randomNode = notWallNodes [Random.Range (0, notWallNodes.Count)];

		return randomNode;
	}

	private List<LFLabyrinthNode> GetAllNotWallNodes()
	{
		List<LFLabyrinthNode> notWallNodes = new List<LFLabyrinthNode> ();

		for (int x = 0; x < _gridSizeX; x++)
		{
			for(int y = 0; y < _gridSizeY; y++)
			{
				if (!_grid [x, y].IsWall)
					notWallNodes.Add (_grid [x, y]);
			}
		}

		return notWallNodes;
	}

	public LFLabyrinthNode RandomFreeNode()
	{
		List<LFLabyrinthNode> notWallNodes = GetAllNotWallNodes();
		LFLabyrinthNode randomNode = notWallNodes [Random.Range (0, notWallNodes.Count)];

		return randomNode;
	}


}
