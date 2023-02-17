using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyBuoyancy : MonoBehaviour
{
    [SerializeField] private WaveHeightCalculator _waveHeightCalculator;
    [SerializeField] private bool _useWaveHeightCalculator;
    private Rigidbody _rb;
    [SerializeField] private Transform[] _buoyancyPoints;

    // Arcade approach
    [SerializeField] private Transform _waterTransform;
    [Tooltip("X represents the absolute value of water depth. Y represents resulting force.")]
    [SerializeField] private AnimationCurve _buoyancyCurve;
    [SerializeField][Range(1, 1000)] private float _buoyancyForceMultiplier;
    [Header("Drag")]
    [SerializeField] private float _airDrag;
    [SerializeField] private float _airAngularDrag;
    [SerializeField] private float _waterDrag;
    [SerializeField] private float _waterAngularDrag;

    [SerializeField] private int _buoyancyPointsUnderwater;
    [HideInInspector] public bool isUnderwater;

    [SerializeField] private bool _debug;

    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        if (_waterTransform == null)
        {
            _waterTransform = GameObject.Find("Water").GetComponent<Transform>();
        }
        _waveHeightCalculator = _waterTransform.gameObject.GetComponent<WaveHeightCalculator>();
        if (_rb == null)
        {
            Debug.LogError("RigidbodyBuoyancy: Rigidbody component is null!");
        }
        if (_buoyancyPoints == null)
        {
            Debug.LogError("RigidbodyBuoyancy: 'ObjectLowestPoint' Transform is null!");
        }
        if (_waveHeightCalculator == null)
        {
            Debug.LogError("RigidbodyBuoyancy: 'WaterLevel' Tranform is null!");
        }
        _rb.centerOfMass = Vector3.zero;
    }

    void FixedUpdate()
    {
        _buoyancyPointsUnderwater = 0;
        for (int i = 0; i < _buoyancyPoints.Length; i++)
        {
            float waterDepth = CalculateWaterDepth(_buoyancyPoints[i]);
            if (waterDepth < 0)
            {
                _buoyancyPointsUnderwater += 1;
                _rb.AddForceAtPosition(new Vector3(0, CalculateBuoyancy(_buoyancyCurve, waterDepth), 0), _buoyancyPoints[i].position, ForceMode.Force);
            }
        }
        if (_buoyancyPointsUnderwater >= Mathf.FloorToInt(_buoyancyPoints.Length / 2))
        {
            isUnderwater = true;
        }
        else
        {
            isUnderwater = false;
        }
    }
    // Change function to run same calculations as waves for water shader, then use that to determine depth
    float CalculateWaterDepth(Transform buoyancyPoint)
    {
        if (_useWaveHeightCalculator)
        {
            return buoyancyPoint.position.y - _waveHeightCalculator.GetWaveHeightAtPosition(buoyancyPoint.position);
        }
        else
        {
            return buoyancyPoint.position.y - _waterTransform.position.y;
        }
    }
    float CalculateBuoyancy(AnimationCurve buoyancyCurve, float waterDepth)
    {
        return buoyancyCurve.Evaluate(Mathf.Abs(waterDepth)) * _buoyancyForceMultiplier;
    }
    void SetAirDragVariables()
    {
        _rb.drag = _airDrag;
        _rb.angularDrag = _airAngularDrag;
    }
    void SetWaterDragVariables()
    {
        _rb.drag = _waterDrag;
        _rb.angularDrag = _waterAngularDrag;
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_debug)
        {
            if (_buoyancyPoints != null)
            {
                for (int i = 0; i < _buoyancyPoints.Length; i++)
                {
                    float waterDepth;
                    if (_useWaveHeightCalculator)
                    {
                        waterDepth = _buoyancyPoints[i].position.y - _waveHeightCalculator.GetWaveHeightAtPosition(_buoyancyPoints[i].position);
                    }
                    else
                    {
                        waterDepth = _buoyancyPoints[i].position.y - _waterTransform.position.y;
                    }
                    if (waterDepth < 0)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(_buoyancyPoints[i].position, 0.1f); // Draw sphere at buoyancy point
                        Gizmos.color = Color.green;
                        if (_useWaveHeightCalculator)
                        {
                            Gizmos.DrawSphere(new Vector3(_buoyancyPoints[i].position.x, _waveHeightCalculator.GetWaveHeightAtPosition(_buoyancyPoints[i].position), _buoyancyPoints[i].position.z), 0.1f); // Draw sphere at water    
                        }
                        else
                        {
                            Gizmos.DrawSphere(new Vector3(_buoyancyPoints[i].position.x, _waterTransform.position.y, _buoyancyPoints[i].position.z), 0.1f); // Draw sphere at water level
                        }
                        UnityEditor.Handles.Label(_buoyancyPoints[i].position, $"Depth: {waterDepth.ToString()} Force: {CalculateBuoyancy(_buoyancyCurve, waterDepth).ToString()}"); 
                        // Draw line from buoyancy point to force vector
                        Debug.DrawLine(_buoyancyPoints[i].position, new Vector3(_buoyancyPoints[i].position.x, CalculateBuoyancy(_buoyancyCurve, waterDepth) / 10, _buoyancyPoints[i].position.z), Color.blue); 
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(_buoyancyPoints[i].position, 0.1f);
                        UnityEditor.Handles.Label(_buoyancyPoints[i].position, $"Depth: {waterDepth.ToString()} Force: N/A");
                        Debug.DrawLine(_buoyancyPoints[i].position, new Vector3(_buoyancyPoints[i].position.x, CalculateBuoyancy(_buoyancyCurve, waterDepth) / 10, _buoyancyPoints[i].position.z), Color.red);
                    }
                }
            }
        }
    }
    #endif
}
