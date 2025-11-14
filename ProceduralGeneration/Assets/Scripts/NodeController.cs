using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public GameObject _nodeColorDisplay;
    public Renderer _nodeRender;
    public Color[] _colorList;

    public NodeGenerator _nodeGenerator;

    public Vector2Int _nodeGridPosition;

    public bool _isCurrentLevel;
    public bool _isNextLevel;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.gameObject.CompareTag("NodeColor"))
            {
                _nodeColorDisplay = child.gameObject;
                break;
            }
        }

        _nodeRender = _nodeColorDisplay.GetComponent<Renderer>();
        _nodeGenerator = GameObject.FindGameObjectWithTag("NodeGeneratorManager").GetComponent<NodeGenerator>();


    }

    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        if (_nodeColorDisplay != null && _isNextLevel)
        {
            _nodeRender.material.color = _colorList[2];
        }
    }

    void OnMouseExit()
    {
        if (_nodeColorDisplay != null && _isNextLevel)
        {
            _nodeRender.material.color = _colorList[1];
        }
    }

    void OnMouseDown()
    {
        if (_nodeGridPosition != null && _isNextLevel)
        {
            Debug.Log(_nodeGenerator._nodeType[_nodeGridPosition.x, _nodeGridPosition.y]);
            _nodeGenerator._currentPlayerNode = _nodeGridPosition;
            _nodeGenerator.GetNextLevels();
        }
    }

}
