using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveHeightCalculator : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] Vector2 _noiseMapScale;
    [SerializeField] float _waveTiling;
    [SerializeField] float _waveOffset;
    [SerializeField] float _waveMin;
    [SerializeField] float _waveMax;
    [Header("Small Waves")]
    [SerializeField] float _wavesSmallScale;
    [SerializeField] float _wavesSmallStrength;
    [SerializeField] Vector2 _wavesSmallVelocity;
    [Header("Large Waves")]
    [SerializeField] float _wavesLargeScale;
    [SerializeField] float _wavesLargeStrength;
    [SerializeField] Vector2 _wavesLargeVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetWaveHeightAtPosition(Vector3 position)
    {
        // N
        // Calculate Small Waves
        // Calculate Large Waves
        // Combine
        return null;
    }
}
