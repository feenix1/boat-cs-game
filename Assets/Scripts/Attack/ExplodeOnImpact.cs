using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
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
        if (explosionPrefab == null)
        {
            Debug.LogError("ExplodeOnImpact: Explosion Prefab is null");
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
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            RigidbodyRedirect rigidbodyRedirect = nearbyObject.gameObject.GetComponent<RigidbodyRedirect>();
            if (rigidbodyRedirect != null)
            {
                Debug.Log($"ExplodeOnImpact: Adding force to RigidbodyRedirect of {nearbyObject.gameObject}");
                rigidbodyRedirect.rigidbodyReference.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            else
            {
                Rigidbody rb = nearbyObject.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Debug.Log($"ExplodeOnImpact: Adding force to Rigidbody of {nearbyObject.gameObject}");
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
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
