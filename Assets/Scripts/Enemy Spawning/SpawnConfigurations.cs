using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnConfigurations : MonoBehaviour
{
    public List<SpawnConfig> _spawnConfigs = new();
    
    [System.Serializable]
    public class SpawnConfig
    {
        public GameObject _enemyPrefab;
        public float _minRadiusFromPlayer;
        public float _maxRadiusFromPlayer;
        [Header("Position")]
        public Vector3 _spawnOffset;
        [Header("Time")]
        public float _secondsBetweenSpawnAttempts;
        [Header("Spawn")]
        public int _maxSpawns;
        public bool _debug;

        [HideInInspector] public float _secondsSinceLastSpawnAttempt;
        [HideInInspector] public List<Vector3> _enemySpawnPositions = new();
        [HideInInspector] public List<GameObject> _activeEnemies = new();
    }
    
}