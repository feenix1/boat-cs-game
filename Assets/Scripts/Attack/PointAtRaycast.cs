using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtRaycast : MonoBehaviour
{
    [SerializeField] private Transform _castOrigin;
    [SerializeField] private Vector3 _castOriginOffset;
    [SerializeField] private Transform _objectToRotate;
    [Header("Raycast")]
    [SerializeField] private float _raycastDistance;
    [SerializeField] private LayerMask _layerMask;
    [Header("Rotation")]
    [SerializeField] private bool _debug;
    private Vector3 _raycastHitPoint;
    // Debounce
    private bool _isRotating;
    private void Start()
    {
        if (_castOrigin == null)
        {
            Debug.LogError($"PointAtRaycast: Cast origin is null!");
        }
        if (_objectToRotate == null)
        {
            Debug.LogError($"PointAtRaycast: Object to rotate is null!");
        }
    }
    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(_castOrigin.position + _castOriginOffset, _castOrigin.forward, out hit, _raycastDistance, _layerMask);
        if (hit.point != null)
        {
            _raycastHitPoint = hit.point;
        }
        if (_raycastHitPoint != Vector3.zero)
        {
            StartCoroutine(RotateToPointAtPosition(_objectToRotate));
        }
    }
    private IEnumerator RotateToPointAtPosition(Transform objectTransfrom)
    {
        _isRotating = true;
        // TODO: Fix this so that the targetRotation is in the local space of the _objectToRotate 
        Quaternion targetRotation = Quaternion.LookRotation(_raycastHitPoint - _objectToRotate.position);
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, _objectToRotate.rotation.eulerAngles.y, _objectToRotate.rotation.eulerAngles.z);
        while (!TransformRotationWithinRange(0.1f, objectTransfrom.rotation, targetRotation))
        {
            objectTransfrom.rotation = targetRotation;
            Debug.Log(objectTransfrom.rotation.eulerAngles + "|" + targetRotation.eulerAngles);
            yield return null;    
        }
        _isRotating = false;
    }
    private bool TransformRotationWithinRange(float angleDifference, Quaternion rotation1, Quaternion rotation2)
    {
        bool xValid = false;
        bool yValid = false;
        bool zValid = false;
        if (Mathf.Abs(rotation1.eulerAngles.x - rotation2.eulerAngles.x) <= angleDifference)
        {
            xValid = true;
        }
        if (Mathf.Abs(rotation1.eulerAngles.y - rotation2.eulerAngles.y) <= angleDifference)
        {
            yValid = true;
        }
        if (Mathf.Abs(rotation1.eulerAngles.z - rotation2.eulerAngles.z) <= angleDifference)
        {
            zValid = true;
        }
        return xValid && yValid && zValid ? true : false;
    }
    private void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.green;
            RaycastHit hit;
            Gizmos.DrawLine(_castOrigin.position + _castOriginOffset, (_castOrigin.position + _castOriginOffset) + (_castOrigin.forward * _raycastDistance));
            if (_isRotating)
            {
                Gizmos.color = Color.blue;
            }
            Gizmos.DrawLine(_objectToRotate.position, _objectToRotate.position + (_objectToRotate.forward * _raycastDistance));
            if (Physics.Raycast(_castOrigin.position + _castOriginOffset, _castOrigin.forward, out hit, _raycastDistance, _layerMask))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(hit.point, 0.5f);
            }
        }
    }
}
