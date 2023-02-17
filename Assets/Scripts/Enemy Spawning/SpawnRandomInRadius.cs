using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomInRadius : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private List<SpawnConfig> _spawnConfigs = new();
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
        foreach (SpawnConfig spawnConfig in _spawnConfigs)
        {
            spawnConfig._secondsSinceLastSpawnAttempt += Time.deltaTime;
            if (spawnConfig._secondsSinceLastSpawnAttempt < spawnConfig._secondsBetweenSpawnAttempts) 
            {
                return;
            }
            spawnConfig._secondsSinceLastSpawnAttempt = 0;
            if (spawnConfig._activeGroups.Count == spawnConfig._maxGroups)
            {
                return;
            }
            spawnConfig._groupSpawnPosition = RandomPointBetweenRadius(spawnConfig._minRadiusFromPlayer, spawnConfig._maxRadiusFromPlayer) + _playerTransform.position;
            spawnConfig._enemySpawnPositions.Clear();
            for (int j = 0; j < spawnConfig._amountPerGroup; j++)
            {
                Vector3 spawnPoint;
                // Doesn't add to the spawnPointsList after 3 tries bc values may make it impossible to get all points in 
                for (int attempts = 0; attempts < 3; attempts++)
                {
                    spawnPoint = RandomPointBetweenRadius(spawnConfig._groupMinSpawnRadius, spawnConfig._groupMaxSpawnRadius) + spawnConfig._groupSpawnPosition;
                    if (PointWithinOtherPointsRadius(spawnPoint, spawnConfig._enemySpawnPositions, spawnConfig._enemySpawnExclusionRadius))
                    {
                        continue;        
                    }
                    else
                    {
                        spawnConfig._enemySpawnPositions.Add(spawnPoint);
                        break;
                    }
                }
            }
            List<GameObject> spawnGroup = new();
            foreach (Vector3 position in spawnConfig._enemySpawnPositions)
            {
                GameObject enemy = Instantiate(spawnConfig._enemyPrefab, position + spawnConfig._spawnOffset, Quaternion.identity);
                spawnGroup.Add(enemy);
            }
            spawnConfig._activeGroups.Add(spawnGroup);
        }
    }
    public void RemoveFromGroup(GameObject enemyInstance)
    {
        foreach (SpawnConfig spawnConfig in _spawnConfigs)
        {
            foreach (List<GameObject> group in spawnConfig._activeGroups)
            {
                foreach (GameObject enemy in group)
                {
                    if (enemyInstance == enemy)
                    {
                        group.Remove(enemy);
                        if (group.Count == 0)
                        {
                            spawnConfig._activeGroups.Remove(group);
                        }
                    }
                }
            }
        }
    }
    bool PointWithinOtherPointsRadius(Vector3 point, List<Vector3> otherPoints, float radius)
    {
        foreach (Vector3 otherPoint in otherPoints)
        {
            if (PointIsInsideRadius(point, otherPoint, radius))
            {
                return true;
            }
        }
        return false;
    }
    Vector3 RandomPointBetweenRadius(float minRadius, float maxRadius)
    {
        Vector3 randomDirectionXZ = new Vector3(Random.Range(-10f, 10.1f), 0, Random.Range(-10f, 10.1f)).normalized;
        float randomDistance = Random.Range(minRadius, maxRadius);
        return randomDirectionXZ * randomDistance;
    }
    bool PointIsInsideRadius(Vector3 point, Vector3 radiusOrigin, float radius)
    {
        Vector3 pointFromRadiusOrigin = point - radiusOrigin;
        if (pointFromRadiusOrigin.magnitude <= radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        foreach (SpawnConfig spawnConfig in _spawnConfigs)
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
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(spawnConfig._groupSpawnPosition, spawnConfig._groupMinSpawnRadius);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(spawnConfig._groupSpawnPosition, spawnConfig._groupMaxSpawnRadius);
                // Individual Spawns
                foreach (Vector3 spawnPosition in spawnConfig._enemySpawnPositions)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(spawnPosition, 5f);
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(spawnPosition, spawnConfig._enemySpawnExclusionRadius);
                }
            }
        }
    }
}
