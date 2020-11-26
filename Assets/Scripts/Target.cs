using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public event System.Action<float> OnHealthPctChanged = delegate { };

    private float currHealth;

    void Start()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        
        currHealth = currHealth - amount;
        if(currHealth <= 0.0f)
        {
            Die();
        }

        float currHealthPct = currHealth / maxHealth;
        OnHealthPctChanged(currHealthPct);
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}
