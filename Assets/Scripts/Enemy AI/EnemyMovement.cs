using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetection))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    private Vector3 _targetPosition;
    [SerializeField] private float _minDistanceToTarget;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private bool _debug;
    private EnemyDetection _enemyDetection;
    private Rigidbody _rigidbody;
    private bool _isMoving;
    void OnValidate()
    {
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }
        _enemyDetection = gameObject.GetComponent<EnemyDetection>();
        if (_enemyDetection == null)
        {
            Debug.LogError($"EnemyMovement: EnemyDetection is null!");
        }
        _enemyDetection._onDetection.AddListener(MoveToTarget);
    }
    public void MoveToTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        if (!_isMoving)
        {
            StartCoroutine(MoveToTargetCoroutine());
        }
    }
    IEnumerator MoveToTargetCoroutine()
    {
        _isMoving = true;
        while ((_targetPosition - transform.position).magnitude > _minDistanceToTarget)
        {
            // Rotate towards target
            Vector3 targetDirection = GetDirectionToPosition(_targetPosition);
            targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _angularSpeed * Time.deltaTime);
            // Apply acceleration
            if (_rigidbody.velocity.magnitude < _maxSpeed)
            {
                _rigidbody.AddRelativeForce(Vector3.forward * _acceleration * Time.deltaTime, ForceMode.Acceleration);
            }
            yield return new WaitForFixedUpdate();
        }
        _isMoving = false;
    }
    Vector3 GetDirectionToPosition(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        return direction.normalized;
    }
    private void OnDrawGizmos()
    {
        if (_debug && _isMoving)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_targetPosition, 0.5f);
            Gizmos.DrawWireSphere(_targetPosition, _minDistanceToTarget);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _targetPosition);
        }
    }
}
