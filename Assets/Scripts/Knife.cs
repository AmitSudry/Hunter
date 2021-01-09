using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private float spawnTime;
    private float delta;

    public float damage = 100.0f;

    void Start()
    {
        spawnTime = Time.time;
        delta = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if(delta>=5.0f)
            UnityEngine.Object.Destroy(gameObject);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;

        if (other.tag == "Enemy")
        {
            Target target = other.gameObject.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        UnityEngine.Object.Destroy(gameObject, 2.0f);
    }
}
