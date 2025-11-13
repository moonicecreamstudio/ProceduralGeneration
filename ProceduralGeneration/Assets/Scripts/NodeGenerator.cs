using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    [Header("Path Grid Parameters")]
    public int _pathHeight;
    public int _pathWidth;
    public float _offset; // How spaced are the nodes from between each other
    public float _numberOfPaths;

    [Header("References")]
    public GameObject _panel; // Visually show the grid
    public GameObject _node;
    public GameObject _pivotPoint; // Starting point
    public LineRenderer _lineRenderer;

    // Index starts at 0, so subtract 1
    // Should try and find a way to standarize this, otherwise it will get confusing
    private GameObject[,] _grid; // 2D array to store nodes
    private int direction;

    // Node IDs
    [System.Serializable]
    public class NodeParameter 
    {
        public string _nodeName;
        public int _nodeID;
        public float _nodeSpawnChance;
    }

    public NodeParameter[] nodeList;


    // Assign 3 bools on a 2D array
    [System.Serializable]
    public struct NodeDirection
    { 
        public bool _hasLeftPath;
        public bool _hasMiddlePath;
        public bool _hasRightPath;
                                  
        public NodeDirection(bool left, bool middle, bool right)
        {
            _hasLeftPath = left;
            _hasMiddlePath = middle;
            _hasRightPath = right;
        }
    }

    public NodeDirection[,] boolsDirection;

    public int[,] _nodeType;
    public GameObject _nodeIcon;

    // Destroy the grid
    public void DestroyNode()
    {
        GameObject[] nodesToDestroy = GameObject.FindGameObjectsWithTag("Node");

        foreach (GameObject node in nodesToDestroy)
        {
            Destroy(node);
        }
    }

    // Generate the grid
    public void GenerateGrid()
    {
        Debug.Log("Generating grid...");

        _grid = new GameObject[_pathWidth, _pathHeight]; // Initialize grid
        _nodeType = new int[_pathWidth, _pathHeight];

        // Centering
        float totalWidth = (_pathWidth - 1) * _offset;
        float startX = _pivotPoint.transform.position.x - totalWidth / 2;

        for (int x = 0; x < _pathWidth; x++)
        {
            for (int z = 0; z < _pathHeight; z++)
            {
                Vector3 spawnPos = new Vector3(startX + x * _offset, 
                                               _pivotPoint.transform.position.y,
                                               _pivotPoint.transform.position.z + z * (_offset * 2));

                var spawnedNode = Instantiate(_node, spawnPos, Quaternion.identity);
                spawnedNode.name = $"Node {x} {z}";

                _grid[x, z] = spawnedNode; // Store in array
                _nodeType[x, z] = nodeList[0]._nodeID; // Change all node types to empty nodes
            }
        }
        Debug.Log("Generation complete.");
    }

    public GameObject GetNode(int x, int z)
    {
        if (x < 0 || x >= _pathWidth || z < 0 || z >= _pathHeight)
        {
            return null;
        }
        return _grid[x, z];
    }

    public void DestroyLines()
    {
        // Delete Lines
        GameObject[] nodesToDestroy = GameObject.FindGameObjectsWithTag("Line");

        foreach (GameObject line in nodesToDestroy)
        {
            Destroy(line);
        }
    }

    public void GeneratePath()
    {
        // Reset colors
        for (int x = 0; x < _pathWidth; x++)
        {
            for (int z = 0; z < _pathHeight; z++)
            {
                GameObject nodes = GetNode(x, z);

                // Color tile
                var node = GetNode(x, z);
                if (node != null)
                {
                    // Loop through node's child objects
                    for (int i = 0; i < node.transform.childCount; i++)
                    {
                        var child = node.transform.GetChild(i);

                        if (child.CompareTag("NodeColor"))
                        {
                            var rend = child.GetComponent<Renderer>();
                            if (rend != null)
                            {
                                rend.material.color = Color.gray;
                            }
                            break;
                        }
                    }
                }
            }
        }

        boolsDirection = new NodeDirection[_pathWidth, _pathHeight];

        for (int p = 0; p < _numberOfPaths; p++) // Number of paths to create
        {
            // Create a list of points for the line renderer
            List<Vector3> points = new List<Vector3>();

            // Random starting position
            int currentX = Random.Range(0, _pathWidth);

            // Choose paths
            for (int z = 0; z < _pathHeight; z++)
            {
                // Stay within the grid
                //currentX = Mathf.Clamp(currentX, 0, _pathWidth - 1);

                // Color tile
                var node = GetNode(currentX, z);
                if (node != null)
                {
                    // Loop through node's child objects
                    for (int i = 0; i < node.transform.childCount; i++)
                    {
                        var child = node.transform.GetChild(i);

                        if (child.CompareTag("NodeColor"))
                        {
                            var rend = child.GetComponent<Renderer>();
                            if (rend != null)
                            {
                                rend.material.color = Color.white;
                            }
                            break;
                        }
                    }
                }

                // Add point
                points.Add(new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z));

                // State the possible directions at the end of a node

                List<int> viablePathChoice = new List<int> { -1, 0, 1 };

                // Check the nodes on the left and right

                if (currentX > 0)
                {
                    if (boolsDirection[currentX - 1, z]._hasRightPath) // If node beside has a right path, don't generate left path
                    {
                        viablePathChoice.Remove(-1);
                    }
                }
                if (currentX == 0) // When node is on the very left, don't generate left
                {
                    viablePathChoice.Remove(-1);
                }

                if (currentX < _pathWidth - 1)
                {
                    if (boolsDirection[currentX + 1, z]._hasLeftPath) // If node beside has a left path, don't generate right path
                    {
                        viablePathChoice.Remove(1);
                    }
                }
                if (currentX == _pathWidth - 1) // When node is on the very right, don't generate right
                {
                    viablePathChoice.Remove(1);
                }


                if (viablePathChoice.Count == 0)
                {
                    viablePathChoice.Add(0);
                }

                int randomIndex = Random.Range(0, viablePathChoice.Count);
                direction = viablePathChoice[randomIndex];

                var nodeEndDirection = boolsDirection[currentX, z];

                if (direction == -1)
                {
                    nodeEndDirection._hasLeftPath = true;
                }

                if (direction == 0)
                {
                    nodeEndDirection._hasMiddlePath = true;
                }

                if (direction == 1)
                {
                    nodeEndDirection._hasRightPath = true;
                }
                boolsDirection[currentX, z] = nodeEndDirection;

                // Change nodes in the path to "Battle type"
                _nodeType[currentX, z] = nodeList[1]._nodeID;

                //Debug.Log("(" + currentX + ", " + z + ") " + boolsDirection[currentX, z]._hasLeftPath);
                //Debug.Log("(" + currentX + ", " + z + ") " + boolsDirection[currentX, z]._hasMiddlePath);
                //Debug.Log("(" + currentX + ", " + z + ") " + boolsDirection[currentX, z]._hasRightPath);


                // Choose the next path to be on the left, middle or right of the previous node.
                currentX += direction;
            }
        
        // Set position for the line renderer

        _lineRenderer.positionCount = points.Count;

        for (int l = 0; l < points.Count; l++)
        {
            _lineRenderer.SetPosition(l, points[l]);
        }

        Instantiate(_lineRenderer, transform);
        }
    }
    public void GenerateNodeTypes()
    {
        for (int x = 0; x < _pathWidth; x++)
        {
            for (int z = 0; z < _pathHeight; z++)
            {
                if (_nodeType[x, z] == nodeList[1]._nodeID) // Look for nodes that have the battle ID
                {
                    // Add all the spawn weights in the array
                    float totalWeight = 0;
                    for (int i = 0; i < nodeList.Length; i++)
                    {
                        totalWeight += nodeList[i]._nodeSpawnChance;
                    }

                    // Roll for the chance of all the nodes
                    float roll = Random.Range(0, totalWeight);
                    float cumulativeWeight = 0;
                    for (int j = 0; j < nodeList.Length; j++)
                    {
                        cumulativeWeight += nodeList[j]._nodeSpawnChance;
                        if (roll < cumulativeWeight)
                        {
                            _nodeType[x, z] = nodeList[j]._nodeID;
                            var node = GetNode(x, z);

                            // Instantiate icon
                            GameObject iconGameObject = Instantiate(_nodeIcon, node.transform);
                            Transform iconTransform = null;

                            // Get the child with the tag "Icon"
                            for (int k = 0; k < iconGameObject.transform.childCount; k++)
                            {
                                var child = iconGameObject.transform.GetChild(k);
                                if (child.CompareTag("Icon"))
                                {
                                    iconTransform = child;
                                    IconBobbler iconBobbler;
                                    SpriteRenderer iconSprite;

                                    iconSprite = iconTransform.GetComponent<SpriteRenderer>();
                                    iconBobbler = iconTransform.GetComponent<IconBobbler>();

                                    // Change the "starting position" of the icon, which is varied by the z position of the node
                                    // "5" is the highest peak, before going back
                                    iconBobbler._current = 1f - Mathf.Abs(z - 5) / 5f;

                                    // Change sprite to match it's ID
                                    iconSprite.sprite = iconBobbler._spriteList[_nodeType[x, z]];
                                    break;
                                }
                            }
                            break; // Exit out of the loop
                        }
                    }
                }
                Debug.Log("(" + x + ", " + z + ") " + "node type: " + _nodeType[x, z]);
            }
        }
    }

}
