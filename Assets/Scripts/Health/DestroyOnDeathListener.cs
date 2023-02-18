using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpawnConfigurations))]
public class DestroyOnDeathListener : MonoBehaviour
{
    SpawnConfigurations _spawnConfigurations;
    [SerializeField] private float _destroyDelay;
    private void Start()
    {
        _spawnConfigurations = gameObject.GetComponent<SpawnConfigurations>();
    }
    public void StartDeath(GameObject enemy)
    {
        StartCoroutine(StartDeathCoroutine(enemy));
    }
    private IEnumerator StartDeathCoroutine(GameObject enemy)
    {
        yield return new WaitForSeconds(_destroyDelay);
        // NOTE: Def not performant to just check all spawn configs but i'm lazy
        foreach (SpawnConfigurations.SpawnConfig spawnConfig in _spawnConfigurations._spawnConfigs)
        {
            if (spawnConfig._activeEnemies.Contains(enemy))
            {
                spawnConfig._activeEnemies.Remove(enemy);
                Destroy(enemy, 2f);
            }
        }
    }
}
