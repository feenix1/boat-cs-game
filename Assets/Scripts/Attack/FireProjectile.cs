using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private bool _usePlayerInput;

    [SerializeField] private Transform _firePoint;
    [SerializeField] private Vector3 _recoilVector;
    [SerializeField] private Rigidbody _rb;
    private Rigidbody _projectileRb;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootSound;

    [SerializeField] private GameObject _shootParticlePrefab;
    private ParticleSystem _fireParticleSystem;

    [SerializeField] private GameObject _projectilePrefab;
    private List<GameObject> _projectiles = new();
    [SerializeField] private float _fireForce;

    private float _shootInput;
    [SerializeField] private float _shootDelay;
    private float _timeSinceLastShot;

    [SerializeField] private bool _debug;

    // Start is called before the first frame update
    void Start()
    {
        if (_rb == null) { _rb = gameObject.GetComponent<Rigidbody>();}
        if (_rb == null)
        {
            Debug.LogError("FireProjectile: Rigidbody is null");
        }
        _projectileRb = _projectilePrefab.GetComponent<Rigidbody>();
        if (_projectileRb == null)
        {
            Debug.LogError("FireProjectile: Projectile Rigidbody is null");
        }
        if (_audioSource == null) {_audioSource = transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>();}
        if (_audioSource == null)
        {
            Debug.LogError("FireProjectile: AudioSource is null");
        }
        else
        {
            _audioSource.clip = _shootSound;
        }
        if (_firePoint == null)
        {
            Debug.LogError("FireProjectile: FirePoint is null");
        }
        if (_projectilePrefab == null)
        {
            Debug.LogError("FireProjectile: Projectile is null");
        }
        if (_shootParticlePrefab == null)
        {
            Debug.LogError("FireProjectile: Shoot Particle is null");
        }
        else
        {
            _fireParticleSystem = _shootParticlePrefab.GetComponent<ParticleSystem>();
            if (_fireParticleSystem == null)
            {
                Debug.LogError("FireProjectile: Shoot Particle System is null");
            }
        }
    }
    void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        if (_usePlayerInput)
        {
            _shootInput = Input.GetAxis("Fire1");
            if (_shootInput != 0)
            {
                Fire();
            }
        }
    }
    public void Fire()
    {
        if (_timeSinceLastShot >= _shootDelay)
        {
            _timeSinceLastShot = 0;
            CreateProjectile();
            PlayFireSound();
            PlayParticle();
        }
    }
    private void CreateProjectile()
    {
        GameObject projectileToAdd = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
        projectileToAdd.transform.parent = GameObject.Find("Projectiles").transform;
        _projectileRb = projectileToAdd.GetComponent<Rigidbody>();
        _rb.AddRelativeForce(_recoilVector, ForceMode.Impulse);
        _projectileRb.AddRelativeForce(new Vector3(0, 0, _fireForce), ForceMode.Impulse);
    }
    private void PlayFireSound()
    {
        _audioSource.clip = _shootSound;
        _audioSource.Play();
    }
    private void PlayParticle()
    {
        GameObject shootParticle = Instantiate(_shootParticlePrefab, _firePoint.position, _firePoint.rotation);
        shootParticle.transform.parent = _rb.gameObject.transform;
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
            Gizmos.DrawCube(_firePoint.position, new Vector3(0.1f, 0.1f, 0.1f));
        }
    }
#endif
}
