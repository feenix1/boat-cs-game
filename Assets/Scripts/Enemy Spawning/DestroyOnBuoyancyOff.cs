using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnConfigurations))]
public class DestroyOnBuoyancyOff : MonoBehaviour
{
    private SpawnConfigurations _spawnConfigurations;
    private List<GameObject> _listToDestroy;
    
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
            }
        }
    }
}
