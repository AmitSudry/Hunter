using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    public Image foreground;

    public float updateSpeedSeconds = 0.5f;
    public Camera cam;

    private void Awake()
    {
        GetComponentInParent<Target>().OnHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foreground.fillAmount;
        float elapsed = 0.0f;

        while(elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foreground.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        foreground.fillAmount = pct;
    }
    
    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(cam.transform);
        transform.Rotate(0, 180, 0);
    }
}
