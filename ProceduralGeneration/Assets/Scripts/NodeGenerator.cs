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
    public GameObject _node; // Visually show the grid
    public GameObject _pivotPoint; // Starting point
    public LineRenderer _lineRenderer;

    // Index starts at 0, so subtract 1
    // Should try and find a way to standarize this, otherwise it will get confusing
    private GameObject[,] _grid; // 2D array to store nodes

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
            }
        }
        Debug.Log("Generation complete.");
    }

    public GameObject GetNode(int x, int z)
    {
        if (x < 0 || x >= _pathWidth || z < 0 || z >= _pathHeight)
            return null;

        return _grid[x, z];
    }

    public void GeneratePath()
    {
        // Reset colors
        for (int x = 0; x < _pathWidth; x++)
        {
            for (int z = 0; z < _pathHeight; z++)
            {
                GameObject nodes = GetNode(x, z);

                Renderer rend = nodes.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.color = Color.gray;
                }
            }
        }

        // Delete Lines
        GameObject[] nodesToDestroy = GameObject.FindGameObjectsWithTag("Line");

        foreach (GameObject line in nodesToDestroy)
        {
            Destroy(line);
        }

        

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
                currentX = Mathf.Clamp(currentX, 0, _pathWidth - 1);

                // Color tile
                var node = GetNode(currentX, z);
                if (node != null)
                {
                    var rend = node.GetComponent<Renderer>();
                    if (rend != null)
                        rend.material.color = Color.red;
                }

                // Add point
                points.Add(new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z));

                // Choose the next path to be on the left, middle or right of the previous node.
                int direction = Random.Range(-1, 2);
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
}
