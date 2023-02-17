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
        [Header("Spawn Groups")]
        public int _amountPerGroup;
        public float _groupMinSpawnRadius;
        public float _groupMaxSpawnRadius;
        public float _enemySpawnExclusionRadius;
        public int _maxGroups;
        public bool _debug;

        [HideInInspector] public float _secondsSinceLastSpawnAttempt;
        public List<List<GameObject>> _activeGroups = new();
        [HideInInspector] public Vector3 _groupSpawnPosition;
        [HideInInspector] public List<Vector3> _enemySpawnPositions = new();
    }
    
}