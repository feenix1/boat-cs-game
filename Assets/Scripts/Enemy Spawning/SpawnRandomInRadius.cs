using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnConfigurations))]
public class SpawnRandomInRadius : MonoBehaviour
{
    private SpawnConfigurations _spawnConfigScript;
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
            if (spawnConfig._activeEnemies.Count >= spawnConfig._maxSpawns)
            {
                return;
            }
            SpawnEnemy(spawnConfig);
        }
    }
    void SpawnEnemy(SpawnConfigurations.SpawnConfig spawnConfig)
    {
        Vector3 spawnPosition = RandomPointBetweenRadius(spawnConfig._minRadiusFromPlayer, spawnConfig._maxRadiusFromPlayer) + _playerTransform.position;
        GameObject enemy = Instantiate(spawnConfig._enemyPrefab, spawnPosition + spawnConfig._spawnOffset, Quaternion.identity, transform);
        enemy.transform.name = $"{spawnConfig._enemyPrefab.name}";
        spawnConfig._activeEnemies.Add(enemy);
    }
    Vector3 RandomPointBetweenRadius(float minRadius, float maxRadius)
    {
        Vector3 randomDirectionXZ = new Vector3(Random.Range(-10f, 10.1f), 0, Random.Range(-10f, 10.1f)).normalized;
        float randomDistance = Random.Range(minRadius, maxRadius);
        return randomDirectionXZ * randomDistance;
    }
    private void OnDrawGizmos()
    {
        foreach (SpawnConfigurations.SpawnConfig spawnConfig in _spawnConfigScript._spawnConfigs)
        {
            if (spawnConfig._debug)
            {
                // Player Radius
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_playerTransform.position, spawnConfig._minRadiusFromPlayer);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_playerTransform.position, spawnConfig._maxRadiusFromPlayer);
                Gizmos.color = Color.blue;
            }
        }
    }
}
