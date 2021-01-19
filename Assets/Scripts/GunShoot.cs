using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class GunShoot : MonoBehaviour
{
    //public Recoil recoilScript;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;
    public Image crosshair;
    
    public GameObject weaponHolder;
    public Animator weapAnimator;

    public float damage = 1.0f;
    public float range = 10f;

    public float fireDelta = 0.5f;

    public int maxAmmo = 6;
    private int currAmmo;
    public float reloadTime = 2.0f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public AudioSource gunShootSound;
    public AudioSource timerSound;

    private bool canShoot = true;

    public bool isAR = false;

    void Start()
    {
        currAmmo = maxAmmo;
        ammoText.SetText(currAmmo.ToString());
        reloadText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        ammoText.SetText(currAmmo.ToString());

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

        if(isAR && Input.GetMouseButton(0) && canShoot)
        {
            Shoot();
            canShoot = false;
            StartCoroutine(ShootDelay());
            return;
        }
        
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Scope s = weaponHolder.GetComponent<Scope>();
            if (s.isScoped)
            {
                s.isScoped = false;
                weapAnimator.SetBool("Scoped", false);
                s.OnUnScoped();
            }
            Shoot();
            canShoot = false;
            StartCoroutine(ShootDelay());
            return;
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
        reloadText.SetText("Reloading...");
    }

    IEnumerator Reload()
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

        ammoText.SetText(currAmmo.ToString());
    }

    void Shoot()
    {
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
            UnityEngine.Object.Destroy(g, 2.0f);
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(fireDelta);
        canShoot = true;
    }
}
