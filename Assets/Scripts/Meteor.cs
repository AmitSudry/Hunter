using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private float spawnTime;
    private float delta;

    private float damage = 10000.0f; //A meteor kills instantly

    public GameObject impactEffect;
    public GameObject fireEffect;

    private Rigidbody rb;

    private float zForce = 0.0f;
    private float xForce = 0.0f;

    public AudioSource explosionSound;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        spawnTime = Time.time;
        delta = 0.0f;

        bool leftTilt = Random.Range(0, 2) == 0 ? true : false;
        bool inwardTilt = Random.Range(0, 2) == 0 ? true : false;

        if (leftTilt)
            xForce = -4.0f;
        else
            xForce = 4.0f;

        if (inwardTilt)
            zForce = -4.0f;
        else
            zForce = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if (delta >= 10.0f)
            UnityEngine.Object.Destroy(gameObject);

        transform.Rotate(0.5f, 0.5f, 1.0f);

        Vector3 force = new Vector3(xForce, 0.0f, zForce);
        rb.AddForce(force);
    }

    void OnTriggerEnter(Collider other)
    {
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

        gameObject.GetComponent<Renderer>().enabled = false;

        explosionSound.Play();

        GameObject g = Instantiate(impactEffect, transform.position, Quaternion.identity);
        UnityEngine.Object.Destroy(g, 2.0f);

        GameObject f = Instantiate(fireEffect, transform.position, Quaternion.identity);
        UnityEngine.Object.Destroy(f, 4.0f);

        UnityEngine.Object.Destroy(gameObject, 4.0f);
    }
}
