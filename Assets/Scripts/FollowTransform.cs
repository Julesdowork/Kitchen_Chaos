using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform _targetTransform;

    void LateUpdate()
    {
        if (_targetTransform == null)
            return;

        transform.position = _targetTransform.position;
        transform.rotation = _targetTransform.rotation;
    }

    public void SetTargetTransform(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }
}
