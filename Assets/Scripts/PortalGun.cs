using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class PortalGun : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public Image crosshair;

    public GameObject weaponHolder;
    public Animator weapAnimator;

    public float range = 100.0f;

    public Camera fpsCam;
    //public ParticleSystem muzzleFlash;

    public AudioSource gunShootSound;

    public GameObject bluePortal;
    public GameObject orangePortal;

    private static GameObject blueInstance = null;
    private static GameObject orangeInstance = null;

    void Start()
    {
        ammoText.SetText("\u221E");
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.SetText("\u221E");

        if (!weapAnimator.GetBool("Scoped"))
            crosshair.enabled = true;

        if (Input.GetMouseButtonDown(0)) //blue portal
        {
            Shoot(true);
        }
        else if (Input.GetMouseButtonDown(1)) //orange portal
        {
            Shoot(false);
        }
    }

    void OnEnable()
    {
        weapAnimator.SetBool("Reloading", false);
        weapAnimator.SetBool("Scoped", false);
        Scope s = transform.parent.gameObject.GetComponent<Scope>(); //Weapon holder Scope script
        s.isScoped = false;
        weapAnimator.SetBool("Scoped", false);
        s.OnUnScoped();
    }

    void Shoot(bool isBlue)
    {
        gunShootSound.Play();

        //muzzleFlash.Play();

        GameObject toSpawn;
        if (isBlue)
            toSpawn = bluePortal;
        else
            toSpawn = orangePortal;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            GameObject instance = Instantiate(toSpawn, hit.point + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
            if (isBlue && blueInstance != null)
            {
                UnityEngine.Object.Destroy(blueInstance);
                blueInstance = instance;
            }
            else if(isBlue && blueInstance == null)
            {
                blueInstance = instance;
            }

            if (!isBlue && orangeInstance != null)
            {
                UnityEngine.Object.Destroy(orangeInstance);
                orangeInstance = instance;
            }
            else if (!isBlue && orangeInstance == null)
            {
                orangeInstance = instance;
            }
        }  
    }
}
