using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class DeathEffects : MonoBehaviour
{
    HealthManager healthManager;
    [SerializeField] GameObject _particlePrefab;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _destructionSound;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = gameObject.GetComponent<HealthManager>();
        healthManager._onHealthZero.AddListener(OnHealthZero);
        if (_particlePrefab == null)
        {
            Debug.LogError("DeathEffects: Particle prefab not set");
        }
        if (_audioSource == null)
        {
            Debug.LogError("DeathEffects: Audio Source not set");
        }
        if (_destructionSound == null)
        {
            Debug.LogError("DeathEffects: Destruction sound not set");
        }
    }
    public void OnHealthZero()
    {
        StartCoroutine(OnHealthZeroCoroutine());
    }
    private IEnumerator OnHealthZeroCoroutine()
    {
        // Particle
        GameObject particlePrefab = Instantiate(_particlePrefab, transform.position, Quaternion.identity);
        particlePrefab.transform.parent = transform;
        particlePrefab.transform.localScale = Vector3.one;
        ParticleSystem prefabParticleSystem = particlePrefab.GetComponent<ParticleSystem>();
        prefabParticleSystem.Play();
        // Audio
        _audioSource.clip = _destructionSound;
        _audioSource.Play();
        yield return new WaitForSeconds(prefabParticleSystem.main.duration);
        Destroy(particlePrefab);
    }
}
