using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    [Header("Path Grid Parameters")]
    public int _pathHeight;
    public int _pathWidth;

    [Header("References")]
    public GameObject panel; // Visually show the grid
    public GameObject _pivotPoint; // Starting point

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Press this button to generate the grid
    public void GenerateGrid()
    {
        Debug.Log("Generating grid...");
        Instantiate(panel, _pivotPoint.transform);
        Debug.Log("Generation complete.");
    }
}
