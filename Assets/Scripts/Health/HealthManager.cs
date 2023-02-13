using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour, IDamageable
{
    public float health;

    public UnityEvent onHealthZero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Damage(float damagePoints)
    {
        if (health - damagePoints <= 0)
        {
            onHealthZero.Invoke();
        }
        else
        {
            health -= damagePoints;
        }
    }
}
