using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject projectile;

    [SerializeField] private float shootInput;
    [SerializeField] private float shootDelay;

    // Start is called before the first frame update
    void Start()
    {
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
        if (projectile == null)
        {
            Debug.LogError("FireProjectile: Projectile is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        shootDelay += Time.deltaTime;    
        shootInput = Input.GetAxis("Fire1");

        if (shootInput >= shootDelay)
        {
            shootDelay = 0;
            Instantiate(projectile, firePoint.position, firePoint.rotation);
        }
    }
}
