﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class LaserShoot : MonoBehaviour
{
    //public Recoil recoilScript;
    public GameObject laserBeam;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;
    public Image crosshair;
    
    public GameObject weaponHolder;
    public Animator weapAnimator;

    public float damage = 1.0f;
    public float range = 10f;

    public float fireDelta = 0.5f;

    public float maxAmmo = 5.0f;
    private float currAmmo;
    public float reloadTime = 2.0f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public AudioSource gunShootSound;
    public AudioSource timerSound;

    private bool canShoot = true;

    void Start()
    {
        currAmmo = maxAmmo;
        ammoText.SetText(((int)currAmmo).ToString());
        reloadText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        if (currAmmo < maxAmmo)
            currAmmo += Time.deltaTime*2;

        ammoText.SetText(((int)currAmmo).ToString());

        if (!weapAnimator.GetBool("Scoped"))
            crosshair.enabled = true;

        reloadText.enabled = false;

        if (currAmmo<=0)
        {
            StartCoroutine(Cooldown());
            return;
        }

        if (Input.GetMouseButton(0) && canShoot)
        {
            laserBeam.GetComponent<ParticleSystem>().Play();
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
        weapAnimator.SetBool("Reloading", false);
        weapAnimator.SetBool("Scoped", false);
        Scope s = transform.parent.gameObject.GetComponent<Scope>(); //Weapon holder Scope script
        s.isScoped = false;
        canShoot = true;
        reloadText.SetText("Cool Down...");
    }

    IEnumerator Cooldown()
    {
        isReloading = true;
        timerSound.Play();

        Scope s = weaponHolder.GetComponent<Scope>();
        if (s.isScoped)
        {
            s.isScoped = false;
            weapAnimator.SetBool("Scoped", false);
            s.OnUnScoped();
        }
        
        crosshair.enabled = false;
        reloadText.enabled = true;

        weapAnimator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f); //Without the animation transition
        weapAnimator.SetBool("Reloading", false);

        yield return new WaitForSeconds(0.25f); //Wait only transition

        currAmmo = maxAmmo;

        reloadText.enabled = false;
        crosshair.enabled = true;

        timerSound.Stop();
        isReloading = false;

        ammoText.SetText(((int)currAmmo).ToString());
    }

    void Shoot()
    {
        //recoilScript.Fire();
        gunShootSound.Play();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
        {
            for(int i=0; i<enemies.Length; i++)
            {
                EnemyMovement em = enemies[i].GetComponent<EnemyMovement>();
                em.followPlayer = true;
            }
        }

        muzzleFlash.Play();

        currAmmo -= Time.deltaTime*5;
        ammoText.SetText(((int)currAmmo).ToString());

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            //GameObject g =  Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //UnityEngine.Object.Destroy(g, 1f);
        }
    }
}