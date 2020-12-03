using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int currWeapon = 0;
    public Animator weapAnimator;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int prevWeapon = currWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            currWeapon = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >=2)
            currWeapon = 1;

        if (prevWeapon != currWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
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
