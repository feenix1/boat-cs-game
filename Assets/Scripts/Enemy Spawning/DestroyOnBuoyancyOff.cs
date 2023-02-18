using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnConfigurations))]
public class DestroyOnBuoyancyOff : MonoBehaviour
{
    private SpawnConfigurations _spawnConfigurations;
    private List<GameObject> _listToDestroy = new();
    
    // Start is called before the first frame update
    private void Start()
    {
        _spawnConfigurations = gameObject.GetComponent<SpawnConfigurations>();
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (SpawnConfigurations.SpawnConfig spawnConfig in _spawnConfigurations._spawnConfigs)
        {
            _listToDestroy.Clear();
            foreach (GameObject enemy in spawnConfig._activeEnemies)
            {
                RigidbodyBuoyancy enemyRigidbodyBuoyancy = enemy.GetComponent<RigidbodyBuoyancy>();
                if (enemyRigidbodyBuoyancy == null)
                {
                    continue;
                }
                if (enemyRigidbodyBuoyancy.enabled == true)
                {
                    continue;
                }
                if (enemyRigidbodyBuoyancy.enabled == false)
                {
                    _listToDestroy.Add(enemy);
                }
            }
            foreach (GameObject enemy in _listToDestroy)
            {
                spawnConfig._activeEnemies.Remove(enemy);
                Destroy(enemy, 2f);
            }
        }
    }
}
