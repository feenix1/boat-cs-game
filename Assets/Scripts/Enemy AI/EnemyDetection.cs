using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DetectionEvent : UnityEvent<Vector3> { }

public class EnemyDetection : MonoBehaviour
{
    public DetectionEvent _onDetection;

    [SerializeField] private float _playerDetectionRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _debug;

    // Start is called before the first frame update
    void Start()
    {
        if (_onDetection == null)
        {
            _onDetection = new DetectionEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, _playerDetectionRadius, _layerMask);
        foreach (Collider collider in overlapColliders)
        {
            GameObject gameObjectOverlapped = collider.gameObject;
            ObjectRedirect gameObjectRedirector = gameObjectOverlapped.GetComponent<ObjectRedirect>();
            if (gameObjectRedirector != null)
            {
                gameObjectOverlapped = gameObjectRedirector.gameObjectRedirect;
            }
            if (gameObjectOverlapped.CompareTag("Player"))
            {
                if (_debug) {Debug.Log($"EnemyDetection: Player detected at position {gameObjectOverlapped.transform.position}!");}
                _onDetection?.Invoke(gameObjectOverlapped.transform.position);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _playerDetectionRadius);
        }
    }
}
