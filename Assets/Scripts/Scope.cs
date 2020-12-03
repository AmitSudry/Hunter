using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Animator animator;
    public GameObject scopeOverlay;
    public GameObject weaponCam;

    public GameObject mainCamera;

    public Camera mainCam;
    public float scopedFOV = 55.0f;

    public bool isScoped = false;
    private float prevFOV; 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isScoped = !isScoped;
            animator.SetBool("Scoped", isScoped);

            if (isScoped)
                StartCoroutine(OnScoped());
            else
                OnUnScoped();    
        }
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(0.15f);

        scopeOverlay.SetActive(true);
        weaponCam.SetActive(false);

        prevFOV = mainCam.fieldOfView;
        mainCam.fieldOfView = scopedFOV;

        MouseLook ml = mainCamera.GetComponent<MouseLook>();
        ml.mouseSensitivity = 80.0f;
    }

    public void OnUnScoped()
    {
        scopeOverlay.SetActive(false);
        weaponCam.SetActive(true);

        mainCam.fieldOfView = prevFOV;

        MouseLook ml = mainCamera.GetComponent<MouseLook>();
        ml.mouseSensitivity = 200.0f;
    }
}
