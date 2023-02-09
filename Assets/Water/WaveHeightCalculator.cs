using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHeightCalculator : MonoBehaviour
{
    [SerializeField] Material _waterMaterial;
    [SerializeField] float _waveBuoyancyScale;
    [Header("Debug")] [SerializeField] bool _debug;
    [SerializeField] int _debugGridSize;
    [SerializeField] float _debugGridScale;

    [Header("Waves")]
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

    private void OnValidate()
    {
        _waterMaterial = GetComponent<Renderer>().sharedMaterial;
        SetVariables();
    }
    void SetVariables()
    {
        _waveTiling = _waterMaterial.GetFloat("_Wave_Tiling");
        _waveOffset = _waterMaterial.GetFloat("_Wave_Offset");
        _waveMin = _waterMaterial.GetFloat("_Wave_Min");
        _waveMax = _waterMaterial.GetFloat("_Wave_Max");
        _wavesSmallScale = _waterMaterial.GetFloat("_Waves_Small_Scale");
        _wavesSmallStrength = _waterMaterial.GetFloat("_Waves_Small_Strength");
        _wavesSmallVelocity = _waterMaterial.GetVector("_Waves_Small_Velocity");
        _wavesLargeScale = _waterMaterial.GetFloat("_Waves_Large_Scale");
        _wavesLargeStrength = _waterMaterial.GetFloat("_Waves_Large_Strength");
        _wavesLargeVelocity = _waterMaterial.GetVector("_Waves_Large_Velocity");
    }
    public float GetWaveHeightAtPosition(Vector3 position)
    {
        Vector2 noiseMapUV;
        noiseMapUV = new Vector2(position.x, position.z) * _waveTiling;
        // Calculate Small Waves
        Vector2 wavesSmallUVOffset = (Time.time / 20) * _wavesSmallVelocity;
        float noiseValueAtUVPlusOffset = UnitySimpleNoiseAtUV(noiseMapUV + wavesSmallUVOffset, _wavesSmallScale);
        float wavesSmall = noiseValueAtUVPlusOffset * _wavesSmallStrength;
        // Calculate Large Waves
        Vector2 wavesLargeUVOffset = (Time.time / 20) * _wavesLargeVelocity;
        noiseValueAtUVPlusOffset = UnitySimpleNoiseAtUV(noiseMapUV + wavesLargeUVOffset, _wavesLargeScale);
        float wavesLarge = noiseValueAtUVPlusOffset * _wavesLargeStrength;
        // Combine
        float waveHeight = wavesSmall + wavesLarge;
        // Clamp
        waveHeight = Mathf.Clamp(waveHeight, _waveMin, _waveMax);
        // Offset
        waveHeight += _waveOffset;
        // Scale
        waveHeight *= _waveBuoyancyScale;
        return waveHeight;
    } //
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_debug)
        {
            for (int x = -_debugGridSize; x < _debugGridSize; x++)
            {
                for (int z = -_debugGridSize; z < _debugGridSize; z++)
                {
                    Vector3 position = new Vector3(x * _debugGridScale, 0, z * _debugGridScale);
                    float waveHeight = GetWaveHeightAtPosition(position);
                    position.y = waveHeight;
                    Gizmos.DrawSphere(position, 0.1f);
                    Gizmos.DrawLine(new Vector3(position.x, 0, position.z), new Vector3(position.x, waveHeight, position.z));
                }
            }
        }
    }
    #endif
    // imported from the generated shader code: convert to C# ----------------------------------
    float float_frac(float x) { return x - Mathf.Floor(x); }
    Vector2 frac(Vector2 x) { return x - new Vector2(Mathf.Floor(x.x), Mathf.Floor(x.y)); }
    float sin(float x) { return Mathf.Sin(x); }
    float dot(Vector2 a, Vector2 b) { return a.x * b.x + a.y * b.y; }
    float float_floor(float x) { return Mathf.Floor(x); }
    Vector2 floor(Vector2 x) { return new Vector2(Mathf.Floor(x.x), Mathf.Floor(x.y)); }
    float float_abs(float x) { return Mathf.Abs(x); }
    Vector2 abs(Vector2 x) { return new Vector2(Mathf.Abs(x.x), Mathf.Abs(x.y)); }
    float pow(float x, float y) { return Mathf.Pow(x, y); }

    float Unity_SimpleNoise_RandomValue_float(Vector2 uv)
    {
        float angle = dot(uv, new Vector2(12.9898f, 78.233f));
        return float_frac(sin(angle) * 43758.5453f);
    }
    float Unity_SimpleNnoise_Interpolate_float(float a, float b, float t)
    {
        return (1.0f - t) * a + (t * b);
    }
    float Unity_SimpleNoise_ValueNoise_float(Vector2 uv)
    {
        Vector2 i = floor(uv);
        Vector2 f = frac(uv);
        f = (f * f) * (new Vector2(3.0f, 3.0f) - new Vector2(2.0f, 2.0f) * f);

        uv = abs(frac(uv) - new Vector2(0.5f, 0.5f));
        Vector2 c0 = i + new Vector2(0.0f, 0.0f);
        Vector2 c1 = i + new Vector2(1.0f, 0.0f);
        Vector2 c2 = i + new Vector2(0.0f, 1.0f);
        Vector2 c3 = i + new Vector2(1.0f, 1.0f);
        float r0 = Unity_SimpleNoise_RandomValue_float(c0);
        float r1 = Unity_SimpleNoise_RandomValue_float(c1);
        float r2 = Unity_SimpleNoise_RandomValue_float(c2);
        float r3 = Unity_SimpleNoise_RandomValue_float(c3);

        float bottomOfGrid = Unity_SimpleNnoise_Interpolate_float(r0, r1, f.x);
        float topOfGrid = Unity_SimpleNnoise_Interpolate_float(r2, r3, f.x);
        float t = Unity_SimpleNnoise_Interpolate_float(bottomOfGrid, topOfGrid, f.y);
        return t;
    }
    float UnitySimpleNoiseAtUV(Vector2 UV, float Scale)
    {
        float t = 0.0f;

        float freq = pow(2.0f, 0);
        float amp = pow(0.5f, 3 - 0);
        t += Unity_SimpleNoise_ValueNoise_float(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        freq = pow(2.0f, 1);
        amp = pow(0.5f, 3 - 1);
        t += Unity_SimpleNoise_ValueNoise_float(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        freq = pow(2.0f, 2);
        amp = pow(0.5f, 3 - 2);
        t += Unity_SimpleNoise_ValueNoise_float(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        return t;
    }
}
