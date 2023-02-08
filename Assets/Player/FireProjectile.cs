using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector3 recoilVector;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject projectile;

    [SerializeField] private float shootInput;
    [SerializeField] private float shootDelay;
    [SerializeField] private float timeSinceLastShot;


    [SerializeField] private bool _debug;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) { rb = gameObject.GetComponent<Rigidbody>();}
        if (rb == null)
        {
            Debug.LogError("FireProjectile: Rigidbody is null");
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
        if (projectile == null)
        {
            Debug.LogError("FireProjectile: Projectile is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;    
        shootInput = Input.GetAxis("Fire1");

        if (shootInput != 0 && timeSinceLastShot >= shootDelay)
        {
            timeSinceLastShot = 0;
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            rb.AddRelativeForce(recoilVector, ForceMode.Impulse);

        }
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
