using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomInRadius : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private List<SpawnConfig> _enemyPrefabs = new();
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
        public float _radiusEnemyExclude;
        public int _maxGroups;
        public bool _debug;

        [HideInInspector] public Vector3 _groupSpawnPosition;
        public float _secondsSinceLastSpawnAttempt;
        [HideInInspector] public Vector3[] _instanceSpawnPosition;
    }

    void Start()
    {
        if (_playerTransform == null)
        {
            Debug.LogError("SpawnRandomInRadius: PlayerTransform is null!");
        }
        if (_groundTransform == null)
        {
            Debug.LogError("SpawnRandomInRadius: GroundTransform is null!");
        }
    }
    void Update()
    {
        for (int i = 0; i < _enemyPrefabs.Count; i++)
        {
            SpawnConfig spawnConfig = _enemyPrefabs[i];
            spawnConfig._secondsSinceLastSpawnAttempt += Time.deltaTime;
            if (spawnConfig._secondsSinceLastSpawnAttempt >= spawnConfig._secondsBetweenSpawnAttempts)
            {
                spawnConfig._secondsSinceLastSpawnAttempt = 0;
                spawnConfig._groupSpawnPosition = RandomPointBetweenRadius(spawnConfig._minRadiusFromPlayer, spawnConfig._maxRadiusFromPlayer) + _playerTransform.position;
                //At group spawn position, find random points in the radius
            }
        }
    }
    Vector3 RandomPointBetweenRadius(float minRadius, float maxRadius)
    {
        Vector3 randomDirectionXZ = new Vector3(Random.Range(-10f, 10.1f), 0, Random.Range(-10f, 10.1f)).normalized;
        float randomDistance = Random.Range(minRadius, maxRadius);
        return randomDirectionXZ * randomDistance;
    }

    private void OnDrawGizmos()
    {
        foreach (SpawnConfig spawnConfig in _enemyPrefabs)
        {
            if (spawnConfig._debug)
            {
                // Player Radius
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_playerTransform.position, spawnConfig._minRadiusFromPlayer);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_playerTransform.position, spawnConfig._maxRadiusFromPlayer);
                // Groups
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(spawnConfig._groupSpawnPosition, 5f);
                Gizmos.DrawWireSphere(spawnConfig._groupSpawnPosition, spawnConfig._groupMinSpawnRadius);
                Gizmos.DrawWireSphere(spawnConfig._groupSpawnPosition, spawnConfig._groupMaxSpawnRadius);
            }
        }
    }
}
