using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LFPathFinder : MonoBehaviour {
	private LFGrid _grid;
	private LFLabyrinthGeneration _labyrinth;

	void Awake()
	{
		_grid = gameObject.GetComponent<LFGrid> ();
		_labyrinth = gameObject.GetComponent<LFLabyrinthGeneration> ();
	}
	
	public List<LFGridNode> FindPath(Vector3 startPos, Vector3 targetPos)
	{
		LFGridNode startNode = _grid.GridNodeFromWorldPosition (startPos);
		LFGridNode targetNode = _grid.GridNodeFromWorldPosition (targetPos);

		return FindNodePath(startNode, targetNode);
	}

	public LFGridNode GetGridNodeWithPosition(Vector3 startPos)
	{
		return _grid.GridNodeFromWorldPosition (startPos);
	}

	public List<LFGridNode> RandomNodePath(Vector3 startPos)
	{
		LFGridNode startNode = _grid.GridNodeFromWorldPosition (startPos);
		LFGridNode targetNode = _grid.GridNodeFromWorldPosition(_labyrinth.RandomFreeNodeWithOutPosition (startPos).WorldPosition);

		return FindNodePath(startNode, targetNode);
	}

	private List<LFGridNode> FindNodePath(LFGridNode startNode, LFGridNode targetNode)
	{
		List<LFGridNode> openSet = new List<LFGridNode> ();
		HashSet<LFGridNode> closeSet = new HashSet<LFGridNode> ();
		openSet.Add (startNode);

		while (openSet.Count > 0) {
			LFGridNode currentNode = openSet [0];

			for (int i = 0; i < openSet.Count; i++) {
				if (openSet [i].FCost <= currentNode.FCost && openSet [i].HCost < currentNode.HCost) {
					currentNode = openSet [i];
				}
			}

			openSet.Remove (currentNode);
			closeSet.Add (currentNode);

			if (currentNode == targetNode) {
				List<LFGridNode> path = RetracePath (startNode, targetNode);

				return path;
			}

			List<LFGridNode> neighbours = _grid.GetNeighbours (currentNode);

			foreach (LFGridNode neighbour in neighbours) {
				if (!neighbour.IsCanWalk || closeSet.Contains (neighbour))
					continue;

				int newMoveCostToNeighbour = currentNode.GCost + GetNodeDistance (currentNode, neighbour);

				if (newMoveCostToNeighbour < neighbour.GCost || !openSet.Contains (neighbour)) {
					neighbour.GCost = newMoveCostToNeighbour;
					neighbour.HCost = GetNodeDistance (neighbour, targetNode);
					neighbour.ParentNode = currentNode;

					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
				}
			}
		}

		List<LFGridNode> emptyPath = new List<LFGridNode>();
		emptyPath.Add(startNode);
			
		return emptyPath;
	}

	private int GetNodeDistance(LFGridNode node0, LFGridNode node1)
	{
		int distX = Mathf.Abs (node0.GridX - node1.GridX);
		int distY = Mathf.Abs (node0.GridY - node1.GridY);

		if (distX > distY) {
			return 14 * distY + 10 * (distX - distY);
		}

		return 14 * distX + 10 * (distY - distX);
	}

	private List<LFGridNode> RetracePath(LFGridNode startNode , LFGridNode endNode)
	{
		List<LFGridNode> path = new List<LFGridNode> ();
		LFGridNode currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.ParentNode;
		}

		path.Reverse ();
		return path;
	}
		
}
