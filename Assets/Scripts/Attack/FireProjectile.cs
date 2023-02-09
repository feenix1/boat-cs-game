using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private bool usePlayerInput;

    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector3 recoilVector;
    [SerializeField] private Rigidbody rb;
    private Rigidbody projectileRb;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    [SerializeField] private GameObject shootParticlePrefab;
    private ParticleSystem fireParticleSystem;

    [SerializeField] private GameObject projectilePrefab;
    private List<GameObject> projectiles = new();
    [SerializeField] private float fireForce;

    private float shootInput;
    [SerializeField] private float shootDelay;
    private float timeSinceLastShot;

    [SerializeField] private bool _debug;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) { rb = gameObject.GetComponent<Rigidbody>();}
        if (rb == null)
        {
            Debug.LogError("FireProjectile: Rigidbody is null");
        }
        projectileRb = projectilePrefab.GetComponent<Rigidbody>();
        if (projectileRb == null)
        {
            Debug.LogError("FireProjectile: Projectile Rigidbody is null");
        }
        if (audioSource == null) {audioSource = transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>();}
        if (audioSource == null)
        {
            Debug.LogError("FireProjectile: AudioSource is null");
        }
        else
        {
            audioSource.clip = shootSound;
        }
        if (firePoint == null)
        {
            Debug.LogError("FireProjectile: FirePoint is null");
        }
        if (projectilePrefab == null)
        {
            Debug.LogError("FireProjectile: Projectile is null");
        }
        if (shootParticlePrefab == null)
        {
            Debug.LogError("FireProjectile: Shoot Particle is null");
        }
        else
        {
            fireParticleSystem = shootParticlePrefab.GetComponent<ParticleSystem>();
            if (fireParticleSystem == null)
            {
                Debug.LogError("FireProjectile: Shoot Particle System is null");
            }
        }
    }
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (usePlayerInput)
        {
            shootInput = Input.GetAxis("Fire1");
            if (shootInput != 0)
            {
                Fire();
            }
        }
    }
    public void Fire()
    {
        if (timeSinceLastShot >= shootDelay)
        {
            timeSinceLastShot = 0;
            CreateProjectile();
            PlayFireSound();
            PlayParticle();
        }
    }
    private void CreateProjectile()
    {
        GameObject projectileToAdd = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectileRb = projectileToAdd.GetComponent<Rigidbody>();
        rb.AddRelativeForce(recoilVector, ForceMode.Impulse);
        projectileRb.AddRelativeForce(new Vector3(0, 0, fireForce), ForceMode.Impulse);
    }
    private void PlayFireSound()
    {
        audioSource.clip = shootSound;
        audioSource.Play();
    }
    private void PlayParticle()
    {
        GameObject shootParticle = Instantiate(shootParticlePrefab, firePoint.position, firePoint.rotation);
        shootParticle.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DeleteParticleAfterDuration(shootParticle));
    }
    IEnumerator DeleteParticleAfterDuration(GameObject particle)
    {
        yield return new WaitForSeconds(particle.GetComponent<ParticleSystem>().main.duration);
        Destroy(particle);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(firePoint.position, new Vector3(0.1f, 0.1f, 0.1f));
        }
    }
#endif
}
