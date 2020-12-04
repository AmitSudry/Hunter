using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunShoot : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;
    public Image crosshair;

    public GameObject weaponHolder;
    public Animator weapAnimator;

    public float damage = 1.0f;
    public float range = 10f;

    // public float fireDelta = 0.5F;
    public int maxAmmo = 6;
    private int currAmmo;
    public float reloadTime = 2.0f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    
    void Start()
    {
        currAmmo = maxAmmo;
        ammoText.SetText(currAmmo.ToString());
        reloadText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.SetText(currAmmo.ToString());
        if (isReloading)
            return;

        if(!weapAnimator.GetBool("Scoped"))
            crosshair.enabled = true;

        reloadText.enabled = false;

        if (currAmmo<=0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currAmmo!=maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Scope s = weaponHolder.GetComponent<Scope>();
            if (s.isScoped)
            {
                s.isScoped = false;
                weapAnimator.SetBool("Scoped", false);
                s.OnUnScoped();
            }
            Shoot();
        }
    }

    void OnEnable()
    {
        isReloading = false;
        //animator.SetBool("Reloading", false);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        Scope s = weaponHolder.GetComponent<Scope>();
        if (s.isScoped)
        {
            s.isScoped = false;
            weapAnimator.SetBool("Scoped", false);
            s.OnUnScoped();
        }
        
        crosshair.enabled = false;
        reloadText.enabled = true;

        yield return new WaitForSeconds(reloadTime);
      
        currAmmo = maxAmmo;

        reloadText.enabled = false;
        crosshair.enabled = true;

        isReloading = false;

        ammoText.SetText(currAmmo.ToString());
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currAmmo--;
        ammoText.SetText(currAmmo.ToString());

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
