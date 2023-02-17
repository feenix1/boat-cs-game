using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtRaycast : MonoBehaviour
{
    [SerializeField] private Transform _castOrigin;
    [SerializeField] private Vector3 _castOriginOffset;
    [SerializeField] private Transform _objectToRotate;
    [SerializeField] private Vector3 _rotationOffset;
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
        // Cast ray;
        Vector3 targetPosition;
        RaycastHit hit;
        if (Physics.Raycast(_castOrigin.position + _castOriginOffset, _castOrigin.forward, out hit, _raycastDistance, _layerMask))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = (_castOrigin.position + _castOriginOffset) + (_castOrigin.forward * _raycastDistance);
            Debug.DrawLine(targetPosition + Vector3.one, targetPosition - Vector3.one); 
        }
        Vector3 directionToTarget = (targetPosition - _objectToRotate.position).normalized;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
        // this should limit the rotation to the X axis, but still add the rotation offset
        rotationToTarget = Quaternion.Euler(rotationToTarget.eulerAngles.x + _rotationOffset.x, rotationToTarget.eulerAngles.y + _rotationOffset.y, rotationToTarget.eulerAngles.z + _rotationOffset.z);
        _objectToRotate.rotation = rotationToTarget;
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
            // Draw ray
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_castOrigin.position + _castOriginOffset, (_castOrigin.forward * _raycastDistance) + (_castOrigin.position + _castOriginOffset));
            RaycastHit hit;
            if (Physics.Raycast(_castOrigin.position + _castOriginOffset, _castOrigin.forward, out hit, _raycastDistance, _layerMask))
            {
                Gizmos.DrawSphere(hit.point, 0.5f);
            }
        }
    }
}
