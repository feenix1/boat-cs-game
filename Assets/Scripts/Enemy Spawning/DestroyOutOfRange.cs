using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnConfigurations))]
public class DestroyOutOfRange : MonoBehaviour 
{
    private SpawnConfigurations _spawnConfigurations;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _safeZoneRadius;
    private void Start()
    {
        _spawnConfigurations = gameObject.GetComponent<SpawnConfigurations>();
        if (_playerTransform == null)
        {
            Debug.LogError("DestroyOutOfRange: Player Transform is not set!");
        }
    }
    private void Update()
    {
        foreach (SpawnConfigurations.SpawnConfig spawnConfig in _spawnConfigurations._spawnConfigs)
        {
            List<GameObject> enemiesToDestroy = new();
            foreach (GameObject enemy in spawnConfig._activeEnemies)
            {   
                if (!(PointIsInsideRadius(enemy.transform.position, _playerTransform.position, _safeZoneRadius)))
                {
                    enemiesToDestroy.Add(enemy);
                }
            }
            foreach (GameObject enemy in enemiesToDestroy)
            {
                spawnConfig._activeEnemies.Remove(enemy);
                Destroy(enemy, 2f);
            }
        }
    }
    private bool PointIsInsideRadius(Vector3 point, Vector3 radiusOrigin, float radius)
    {
        Vector3 vectorToPointFromOrigin = point - radiusOrigin;
        if (vectorToPointFromOrigin.magnitude <= radius)
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_playerTransform.position, _safeZoneRadius);
    }
}