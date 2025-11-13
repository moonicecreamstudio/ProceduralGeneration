using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconBobbler : MonoBehaviour
{
    public AnimationCurve _curve;
    public Vector3 _startPosition;
    public Vector3 _maxHeight;
    public Vector3 _minHeight;
    public float _speed;
    public GameObject _icon;
    public float _current, _target;

    public Sprite[] _spriteList;
    public Color[] _spriteColorList;

    // Start is called before the first frame update
    void Start()
    {
        _icon = gameObject;
        _startPosition = _icon.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _current = Mathf.MoveTowards(_current, _target, _speed * Time.deltaTime);

        _icon.transform.position = Vector3.Lerp(_startPosition + _maxHeight, _startPosition - _minHeight, _curve.Evaluate(_current));

        // When icon reaches max or min height, change the target position
        if(_icon.transform.position == _startPosition + _maxHeight || _icon.transform.position == _startPosition - _minHeight)
        {
            ReachedDestination();
        }
    }
    // Flipflop between 0 and 1
    public void ReachedDestination()
    {
        _target = _target == 0 ? 1 : 0;
    }
}
