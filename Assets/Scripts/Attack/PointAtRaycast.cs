using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtRaycast : MonoBehaviour
{
    [SerializeField] private Transform _castingTransform;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private LayerMask _raycastLayerMask;
    [SerializeField] private float _raycastDistance;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotationClampMin;
    [SerializeField] private float _rotationClampMax;
    [SerializeField] private float _rotationOffset;
    [SerializeField] private bool _debug;
    bool _isRotating;
    // Start is called before the first frame update
    void Start()
    {
        if (_castingTransform == null)
        {
            Debug.LogError("PointAtRaycast: Casting Transform is null");
        }
        if (_targetTransform == null)
        {
            Debug.LogError("PointAtRaycast: Target Transform is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_debug)
        {
            Debug.DrawRay(_castingTransform.position, _castingTransform.forward * _raycastDistance, Color.red);
        }
        if (!_isRotating)
        {
            RaycastHit hit;
            Physics.Raycast(_castingTransform.position, _castingTransform.forward, out hit, _raycastDistance, _raycastLayerMask);
            if (hit.collider != null)
            {
                Vector3 hitDirection = hit.point - _targetTransform.position;
                Vector3 hitRotation = Quaternion.LookRotation(hitDirection).eulerAngles; 
                hitRotation.x = Mathf.Clamp(hitRotation.x, _rotationClampMin, _rotationClampMax);
                hitRotation.y = Mathf.Clamp(hitRotation.y, _rotationClampMin, _rotationClampMax);
                hitRotation.z = Mathf.Clamp(hitRotation.z, _rotationClampMin, _rotationClampMax);
                print($"Hit Rotation: {hitRotation}");
                StartCoroutine(RotateToEuler(_targetTransform, hitRotation, _rotationSpeed));
            }
            // TODO: Have this code cast a ray forwards from the player, then align the target transform to the point of impact
        }
    }
    private IEnumerator RotateToEuler(Transform targetTransform, Vector3 eulerTargetRotation, float rotationSpeed)
    {
        _isRotating = true;
        Quaternion targetRotation = Quaternion.Euler(eulerTargetRotation);
        float lerpTime = 0;
        while (lerpTime <= 1)
        {
            targetTransform.rotation = Quaternion.Lerp(targetTransform.rotation, targetRotation, lerpTime);
            lerpTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        _isRotating = false;
    }
}
