using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent constraint is being weird, hence this. Minimial functionality
// for minimal time
public class OffsetCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _target = default;
    [SerializeField]
    private Vector3 _offset = Vector3.zero;

    private void LateUpdate()
    {
        transform.position = _target.position + _offset;
    }
}
