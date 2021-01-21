using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class KnifeThrow : MonoBehaviour
{
    public GameObject knife;
    public float speed = 10.0f;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;
    public Image crosshair;

    public GameObject weaponHolder;
    public Animator weapAnimator;

    public int maxAmmo = 1;
    private int currAmmo;
    public float reloadTime = 2.0f;
    private bool isReloading = false;

    public Camera fpsCam;

    public AudioSource gunShootSound;
    public AudioSource timerSound;

    private bool canShoot = true;

    void Start()
    {
        currAmmo = maxAmmo;
        ammoText.SetText(currAmmo.ToString());
        reloadText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(1.0f, 0.0f, 0.0f);

        if (isReloading)
            return;

        ammoText.SetText(currAmmo.ToString());

        if (!weapAnimator.GetBool("Scoped"))
            crosshair.enabled = true;

        reloadText.enabled = false;

        if (currAmmo <= 0)
        {
            StartCoroutine(Reload());
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
        reloadText.SetText("Just A Sec");
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
        canShoot = true;
        gameObject.GetComponent<Renderer>().enabled = true;
        isReloading = false;

        ammoText.SetText(currAmmo.ToString());
    }

    void Shoot()
    {
        gunShootSound.Play();

        gameObject.GetComponent<Renderer>().enabled = false;

        GameObject g = Instantiate(knife, gameObject.transform.position, gameObject.transform.rotation);
        Rigidbody rb = g.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;

         
        rb.AddForce(fpsCam.transform.forward * speed, ForceMode.Impulse);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                EnemyMovement em = enemies[i].GetComponent<EnemyMovement>();
                em.followPlayer = true;
            }
        }

        currAmmo--;
        ammoText.SetText(currAmmo.ToString());
    }
}