using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField] private float _explosionDamageMin, _explosionDamageMax;
    [SerializeField] private GameObject _explosionParticlePrefab;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _sinkSound;
    [SerializeField] private AudioClip _explosionSound;

    [SerializeField] private bool _debug;

    // Start is called before the first frame update
    void Start()
    {
        if (_audioSource == null) {_audioSource = gameObject.GetComponent<AudioSource>();}
        if (_audioSource == null)
        {
            Debug.LogError("ExplodeOnImpact: AudioSource is null");
        }
        else
        {
            _audioSource.clip = _sinkSound;
        }
        if (_explosionParticlePrefab == null)
        {
            Debug.LogError("ExplodeOnImpact: Explosion Particle Prefab is null");
        }    
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Sink();
        }
        else
        {
            Explode();
        }
    }
    void Sink()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        _audioSource.clip = _sinkSound;
        _audioSource.Play();
        Destroy(gameObject, gameObject.GetComponent<ParticleSystem>().main.startLifetime.constant);
    }
    void Explode()
    {
        GameObject explosion = Instantiate(_explosionParticlePrefab, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (Collider nearbyCollider in colliders)
        {
            GameObject hitGameObject = nearbyCollider.gameObject;
            if (hitGameObject == gameObject)
            {
                continue;
            }
            // Script added to colliders that are children of gameObjects
            ObjectRedirect objectRedirect = hitGameObject.GetComponent<ObjectRedirect>();
            if (objectRedirect != null)
            {
                hitGameObject = objectRedirect.gameObjectRedirect;
            }
            // Damage - Uses interfaces; is that neccessary?
            HealthManager healthManager = hitGameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.GetComponent<IDamageable>().Damage(UnityEngine.Random.Range(_explosionDamageMin, _explosionDamageMax));
            }
            // Physics
            Rigidbody rb = hitGameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }

        }
        _audioSource.clip = _explosionSound;
        _audioSource.Play();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject, _audioSource.clip.length);
    }
    private void OnDrawGizmosSelected()
    {
        if (!_debug) { return; }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
