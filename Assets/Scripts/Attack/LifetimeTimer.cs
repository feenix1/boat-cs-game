using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeTimer : MonoBehaviour
{
    [SerializeField] private float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterTime(lifetime));   
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
