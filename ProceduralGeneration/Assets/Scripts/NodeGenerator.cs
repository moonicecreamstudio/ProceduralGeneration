using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    [Header("Path Grid Parameters")]
    public int _pathHeight;
    public int _pathWidth;
    public float _offset; // How spaced are the tiles from between each other

    [Header("References")]
    public GameObject _panel; // Visually show the grid
    public GameObject _pivotPoint; // Starting point

    void Start()
    {
        float xPivot = _pivotPoint.transform.position.x;
    }

    void Update()
    {
        
    }
    // Destroy the grid
    public void DestroyPanels()
    {
        GameObject[] panelsToDestroy = GameObject.FindGameObjectsWithTag("Panel");

        foreach (GameObject panel in panelsToDestroy)
        {
            Destroy(panel);
        }
    }

    // Generate the grid
    public void GenerateGrid()
    {
        Debug.Log("Generating grid...");
        //Instantiate(_panel, _pivotPoint.transform);
        Debug.Log(_panel.transform.localScale.x);

        // Centering
        float totalWidth = (_pathWidth - 1) * _offset;
        float startX = _pivotPoint.transform.position.x - totalWidth / 2;

        for (int x = 0; x < _pathWidth; x++)
        {
            for (int z = 0; z < _pathHeight; z++)
            {
                Vector3 spawnPos = new Vector3(startX + x * _offset, 
                                               _pivotPoint.transform.position.y,
                                               _pivotPoint.transform.position.z + z * _offset);

                var spawnedTile = Instantiate(_panel, spawnPos, Quaternion.identity);
                spawnedTile.name = $"Tile {x} {z}";
            }
        }
        Debug.Log("Generation complete.");
    }

}
