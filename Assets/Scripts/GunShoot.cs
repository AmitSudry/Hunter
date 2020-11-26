//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{

    public float damage = 1.0f;
    public float range = 10f;

   // public float fireDelta = 0.5F;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject g =  Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            UnityEngine.Object.Destroy(g, 1f);
        }     

    }
    
}
