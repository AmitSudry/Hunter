using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StaminaBar : MonoBehaviour
{
    public Image foreground;

    public float updateSpeedSeconds = 0.5f;

    private void Awake()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().OnStaminaPctChanged += HandleHealthChanged;
        }
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
}
