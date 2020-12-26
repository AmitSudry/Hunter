using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager aud;
    public AudioSource themeSound;

    // Start is called before the first frame update
    void Awake()
    {
        if (aud == null)
            aud = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        themeSound.Play();
    }
}
