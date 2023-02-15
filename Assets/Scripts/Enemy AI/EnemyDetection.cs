using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private float _playerDetectionRadius;
    [SerializeField] private float _timeDetectedBeforeMoveTo;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _debug;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, _playerDetectionRadius, _layerMask);
        foreach (Collider collider in overlapColliders)
        {
            // ha be bop TODO:
        }
    }
}
