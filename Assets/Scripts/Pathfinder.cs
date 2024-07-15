using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    IDictionary<Vector3, Vector3> parentNodes = new Dictionary<Vector3, Vector3>();
    public IDictionary<Vector3, bool> walkablePositions;
    NodeNetworkCreator nodeNetworkCreator;
    IList<Vector3> path;
    bool movePathfinder = false;
    int currentIndex;

    // Use this for initialization
    void Start()
    {
        nodeNetworkCreator = GameObject.Find("NodeNetwork").GetComponent<NodeNetworkCreator>();
        walkablePositions = nodeNetworkCreator.walkablePositions;
    }

    // Update is called once per frame
    void Update()
    {
        // Hacky way to move the pathfinder along the path.
        if (movePathfinder)
        {
            float step = Time.deltaTime * 5;
            transform.position = Vector3.MoveTowards(transform.position, path[currentIndex], step);
            if (transform.position.Equals(path[currentIndex]) && currentIndex >= 0)
                currentIndex--;
            if (currentIndex < 0)
                movePathfinder = false;
        }
    }

    // Depth first search of the graph.
    // Populates IList<Vector3> path with a valid solution to the goalPosition.
    // Returns the goalPosition if a solution is found.
    // Returns the startPosition if no solution is found.
    Vector3 FindPathDFS(Vector3 startPosition, Vector3 goalPosition)
    {
        Stack<Vector3> stack = new Stack<Vector3>();
        HashSet<Vector3> exploredNodes = new HashSet<Vector3>();
        stack.Push(startPosition);

        while (stack.Count != 0)
        {
            Vector3 currentNode = stack.Pop();
            if (currentNode == goalPosition)
            {
                return currentNode;
            }

            IList<Vector3> nodes = GetWalkableNodes(currentNode);

            foreach (Vector3 node in nodes)
            {
                if (!exploredNodes.Contains(node))
                {
                    // Mark the node as explored
                    exploredNodes.Add(node);

                    // Store a reference to the previous node
                    parentNodes.Add(node, currentNode);

                    // Add this to the stack of nodes to examine
                    stack.Push(node);
                }
            }
        }

        return startPosition;
    }

    IList<Vector3> GetWalkableNodes(Vector3 currentNode)
    {
        IList<Vector3> walkableNodes = new List<Vector3>();

        IList<Vector3> possibleNodes = new List<Vector3>() {
            new Vector3(currentNode.x + 1, currentNode.y, currentNode.z),
            new Vector3(currentNode.x - 1, currentNode.y, currentNode.z),
            new Vector3(currentNode.x, currentNode.y, currentNode.z + 1),
            new Vector3(currentNode.x, currentNode.y, currentNode.z - 1)
        };

        foreach (Vector3 node in possibleNodes)
        {
            if (CanMove(node))
            {
                walkableNodes.Add(node);
            }
        }

        return walkableNodes;
    }

    bool CanMove(Vector3 nextPosition)
    {
        return walkablePositions.ContainsKey(nextPosition) ? walkablePositions[nextPosition] : false;
    }

    public void DisplayPath()
    {
        parentNodes = new Dictionary<Vector3, Vector3>();
        path = FindPath();

        Sprite exploredTile = Resources.Load<Sprite>("Path");
        Sprite victoryTile = Resources.Load<Sprite>("Victory");

        foreach (Vector3 node in path)
        {
            nodeNetworkCreator.nodeReference[node].GetComponent<SpriteRenderer>().sprite = victoryTile;
        }

        nodeNetworkCreator.nodeReference[path[0]].GetComponent<SpriteRenderer>().sprite = victoryTile;

        currentIndex = path.Count - 1;
    }

    public void MovePathfinder()
    {
        movePathfinder = true;
    }

    IList<Vector3> FindPath()
    {
        IList<Vector3> path = new List<Vector3>();
        Vector3 goal = FindPathDFS(this.transform.localPosition, GameObject.Find("Goal").transform.localPosition);

        if (goal == this.transform.localPosition)
        {
            // No solution was found.
            return null;
        }

        Vector3 currentNode = goal;
        while (currentNode != this.transform.localPosition)
        {
            path.Add(currentNode);
            currentNode = parentNodes[currentNode];
        }

        return path;
    }
}
