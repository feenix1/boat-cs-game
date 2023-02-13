using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class DeathEffects : MonoBehaviour
{
    HealthManager healthManager;
    [SerializeField] GameObject deathParticlePrefab;
    // Start is called before the first frame update
    void Start()
    {
        healthManager = gameObject.GetComponent<HealthManager>();
        // TODO: make addlistener add coroutine
        healthManager.onHealthZero.AddListener(() => StartCoroutine(OnHealthZero));
    }
    public IEnumerator OnHealthZero()
    {
        return null;
    }
}
