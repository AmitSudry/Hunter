using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionEffect;
    public float delay = 3.0f;

    public float force = 10.0f;
    public float radius = 15.0f;
    public float damage = 125.0f;

    public AudioSource explosionSound;

    private float timeFromLaunch = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timeFromLaunch += Time.deltaTime;

        if (timeFromLaunch >= delay)
            Explode();
    }

    public void Explode()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider near in colls)
        {
            if (near.transform == gameObject.transform)
            {
                continue;
            }

            if (near.gameObject.tag == "Player")
            {
                continue;
            }
                

            Rigidbody rb = near.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(force, transform.position, radius, 10.0f, ForceMode.Impulse);

            Target target = near.GetComponent<Target>();

            if (target != null)
                target.TakeDamage(damage);
            /*
            if(Vector3.Distance(near.transform.position, transform.position) < 3.0f)
            {

            }
            
            Grenade nearByGrenade = near.GetComponent<Grenade>();

            if (nearByGrenade != null)
                nearByGrenade.Explode();
            */
        }

        GameObject g = Instantiate(explosionEffect, transform.position, transform.rotation);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                EnemyMovement em = enemies[i].GetComponent<EnemyMovement>();
                em.followPlayer = true;
            }
        }

        UnityEngine.Object.Destroy(g, 1.0f);
        UnityEngine.Object.Destroy(gameObject);
    }
}
