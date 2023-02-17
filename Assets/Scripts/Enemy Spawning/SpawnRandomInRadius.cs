using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnConfigurations))]
public class SpawnRandomInRadius : MonoBehaviour
{
    [SerializeField] private SpawnConfigurations _spawnConfigScript;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _groundTransform;
    private void OnValidate()
    {
        _spawnConfigScript = gameObject.GetComponent<SpawnConfigurations>();
    }
    void Start()
    {
        _spawnConfigScript = gameObject.GetComponent<SpawnConfigurations>();
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
        foreach (SpawnConfigurations.SpawnConfig spawnConfig in _spawnConfigScript._spawnConfigs)
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
            SpawnGroup(spawnConfig);
        }
    }
    void SpawnGroup(SpawnConfigurations.SpawnConfig spawnConfig)
    {
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
            GameObject enemy = Instantiate(spawnConfig._enemyPrefab, position + spawnConfig._spawnOffset, Quaternion.identity, transform);
            enemy.transform.name = $"{spawnConfig._enemyPrefab.name} - Group: {spawnConfig._activeGroups.Count} Index: {spawnGroup.Count}";
            spawnGroup.Add(enemy);
        }
        spawnConfig._activeGroups.Add(spawnGroup);
        
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
        foreach (SpawnConfigurations.SpawnConfig spawnConfig in _spawnConfigScript._spawnConfigs)
        {
            Debug.Log($"SpawnManager: Groups Active: {spawnConfig._activeGroups.Count}");
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
