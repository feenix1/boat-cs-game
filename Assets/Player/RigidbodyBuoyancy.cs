using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyBuoyancy : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private Transform[] _buoyancyPoints;
    [SerializeField] private Transform _waterLevel;

    // Arcade approach
    [Tooltip("X represents the absolute value of water depth. Y represents resulting force.")]
    [SerializeField] private AnimationCurve _buoyancyCurve;
    [SerializeField][Range(1, 1000)] private float _buoyancyForceMultiplier;
    [Header("Drag")]
    [SerializeField] private float _airDrag;
    [SerializeField] private float _airAngularDrag;
    [SerializeField] private float _waterDrag;
    [SerializeField] private float _waterAngularDrag;

    [SerializeField] private int _buoyancyPointsUnderwater;
    public bool isUnderwater;

    [SerializeField] private bool _debug;

    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("RigidbodyBuoyancy: Rigidbody component is null!");
        }
        if (_buoyancyPoints == null)
        {
            Debug.LogError("RigidbodyBuoyancy: 'ObjectLowestPoint' Transform is null!");
        }
        if (_waterLevel == null)
        {
            Debug.LogError("RigidbodyBuoyancy: 'WaterLevel' Tranform is null!");
        }
    }

    void FixedUpdate()
    {
        _buoyancyPointsUnderwater = 0;
        for (int i = 0; i < _buoyancyPoints.Length; i++)
        {
            float waterDepth = _buoyancyPoints[i].position.y - _waterLevel.position.y;
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
    private void OnDrawGizmos()
    {
        if (_debug)
        {
            if (_buoyancyPoints != null)
            {
                for (int i = 0; i < _buoyancyPoints.Length; i++)
                {
                    float waterDepth = _buoyancyPoints[i].position.y - _waterLevel.position.y;
                    if (waterDepth < 0)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(_buoyancyPoints[i].position, 0.1f);
                        UnityEditor.Handles.Label(_buoyancyPoints[i].position, $"Depth: {waterDepth.ToString()} Force: {CalculateBuoyancy(_buoyancyCurve, waterDepth).ToString()}");
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(_buoyancyPoints[i].position, 0.1f);
                        UnityEditor.Handles.Label(_buoyancyPoints[i].position, $"Depth: {waterDepth.ToString()} Force: N/A");
                    }
                }
            }
        }
    }
}
