using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField] private float explosionDamage;
    [SerializeField] private GameObject explosionParticlePrefab;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sinkSound;
    [SerializeField] private AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null) {audioSource = gameObject.GetComponent<AudioSource>();}
        if (audioSource == null)
        {
            Debug.LogError("ExplodeOnImpact: AudioSource is null");
        }
        else
        {
            audioSource.clip = sinkSound;
        }
        if (explosionParticlePrefab == null)
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
        else if (!other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }
    void Sink()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        audioSource.clip = sinkSound;
        audioSource.Play();
        Destroy(gameObject, gameObject.GetComponent<ParticleSystem>().main.startLifetime.constant);
    }
    void Explode()
    {
        GameObject explosion = Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
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
                Debug.Log($"ExplodeOnImpact: Damaging HealthManager of {hitGameObject}");
                healthManager.GetComponent<IDamageable>().Damage(explosionDamage);
            }
            // Physics
            Rigidbody rb = hitGameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log($"ExplodeOnImpact: Adding force to Rigidbody of {hitGameObject}");
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

        }
        audioSource.clip = explosionSound;
        audioSource.Play();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject, audioSource.clip.length);
    }
}
