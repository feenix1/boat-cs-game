using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _minDistanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null)
        {
            Debug.LogError($"EnemyMovement: NavMeshAgent is null!");
        }
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        Vector3 vectorToPlayer = targetPosition - transform.position;
        Vector3 vectorToPlayerDirection = vectorToPlayer.normalized;
        float vectorToPlayerDistance = vectorToPlayer.magnitude;
        Vector3 destinationVector = vectorToPlayerDirection * (vectorToPlayerDistance - _minDistanceToTarget);
        _navMeshAgent.destination = destinationVector;
    }
}
