using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scope : MonoBehaviour
{
    public Animator animator;
    public GameObject weaponCam;
    public Image crosshair;

    public GameObject mainCamera;

    public Camera mainCam;
    public float scopedFOV = 15.0f;

    public bool isScoped = false;
    private float normalFOV = 60.0f;

    public GameObject[] scopes;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isScoped = !isScoped;
            animator.SetBool("Scoped", isScoped);

            if (isScoped)
            {
                crosshair.enabled = false;
                StartCoroutine(OnScoped());        
            }
            else
            {
                //The crosshair bug could be fixed if I add a check for
                //weapon reload state
                crosshair.enabled = true;
                OnUnScoped();
            }
                  
        }
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(0.15f);

        WeaponSwitching w = gameObject.GetComponent<WeaponSwitching>();
        for (int i = 0; i < 3; i++)
        {
            if(w.currWeapon==i)
                scopes[i].SetActive(true);
            else
                scopes[i].SetActive(false);
        }

        weaponCam.SetActive(false);

        mainCam.fieldOfView = scopedFOV;

        MouseLook ml = mainCamera.GetComponent<MouseLook>();
        ml.mouseSensitivity = 80.0f;
    }

    public void OnUnScoped()
    {
        WeaponSwitching w = gameObject.GetComponent<WeaponSwitching>();
        for(int i=0; i<3; i++)
        {
            scopes[i].SetActive(false);
        }
        
        weaponCam.SetActive(true);

        mainCam.fieldOfView = normalFOV;

        MouseLook ml = mainCamera.GetComponent<MouseLook>();
        ml.mouseSensitivity = 200.0f;
    }
}
