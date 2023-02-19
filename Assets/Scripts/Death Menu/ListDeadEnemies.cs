using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListDeadEnemies : MonoBehaviour
{
    [SerializeField] private GameObject _enemiesParent;
    [HideInInspector] public List<GameObject> _deadEnemies = new();
    // Start is called before the first frame update
    private void Start()
    {
        if (_enemiesParent == null)
        {
            Debug.LogError("ListDeadEnemies: Enemies Parent is null!");
        }
        _deadEnemies.Clear();
    }
    // Update is called once per frame
    private void Update()
    {
        foreach (Transform enemyTransform in _enemiesParent.transform)
        {
            if (_deadEnemies.Contains(enemyTransform.gameObject))
            {
                continue;
            }
            HealthManager enemyHealthManager = enemyTransform.gameObject.GetComponent<HealthManager>();
            if (enemyHealthManager == null)
            {
                continue;
            }
            if (enemyHealthManager._currentHealth <= 0)
            {
                _deadEnemies.Add(enemyTransform.gameObject);
            }
        }
    }
}
