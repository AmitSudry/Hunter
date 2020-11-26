using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100.0f;

    public void TakeDamage(float amount)
    {
        health = health - amount;
        if(health<=0.0f)
        {
            Die();
        }
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}
