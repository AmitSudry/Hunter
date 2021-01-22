using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WeaponSwitching : MonoBehaviour
{
    public int currWeapon = 0;
    public int numOfWeapons = 8;

    public Animator weapAnimator;

    public AudioSource weaponSwitchSound;
    public AudioSource timerSound;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int prevWeapon = currWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (currWeapon != numOfWeapons - 1)
                currWeapon++;
            else
                currWeapon = 0;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (currWeapon != 0)
                currWeapon--;
            else
                currWeapon = numOfWeapons - 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            currWeapon = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            currWeapon = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
            currWeapon = 2;

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
            currWeapon = 3;

        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
            currWeapon = 4;

        if (Input.GetKeyDown(KeyCode.Alpha6) && transform.childCount >= 6)
            currWeapon = 5;

        if (Input.GetKeyDown(KeyCode.Alpha7) && transform.childCount >= 7)
            currWeapon = 6;

        if (Input.GetKeyDown(KeyCode.Alpha8) && transform.childCount >= 8)
            currWeapon = 7;

        if (prevWeapon != currWeapon)
        {
            SelectWeapon();
        }
            
    }

    void SelectWeapon()
    {
        timerSound.Stop();
        weaponSwitchSound.Play();
        Scope s = gameObject.GetComponent<Scope>();
        if (s.isScoped)
        {
            s.isScoped = false;
            weapAnimator.SetBool("Scoped", false);
            s.OnUnScoped();
        }
 
        int i = 0;
        foreach (Transform weap in transform)
        {
            if (i == currWeapon)
                weap.gameObject.SetActive(true);
            else
                weap.gameObject.SetActive(false);
            i++;
        }
    }
}
