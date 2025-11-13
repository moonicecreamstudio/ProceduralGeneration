using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject _camera;
    public float _moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraControls();
    }

    void CameraControls()
    {
        Vector3 cameraPosition = _camera.transform.position;

        if (Input.GetKey(KeyCode.UpArrow) || (Input.GetKey(KeyCode.W)))
        {
            cameraPosition.z += _moveSpeed * Time.deltaTime;
            _camera.transform.position = cameraPosition;
        }
        if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S)))
        {
            cameraPosition.z -= _moveSpeed * Time.deltaTime;
            _camera.transform.position = cameraPosition;
        }
    }

}
