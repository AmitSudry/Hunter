using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 800.0f;
    public float sideForce = 800.0f;
    public float upForce = 2000.0f;
    public bool isOnGround = false;

    public event System.Action<float> OnStaminaPctChanged = delegate { };
    public float maxTimeStamina = 5.0f;
    private float leftStamina;
    private bool canRegainStamina = true;

    void Start()
    {
        leftStamina = maxTimeStamina;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Jump");
        
        Vector3 move = transform.right * x * sideForce + transform.forward * z * forwardForce;
        
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            Vector3 jump = transform.up * y * upForce;
            rb.AddForce(jump);
        }

        if (leftStamina <= 0.0f && canRegainStamina)
        {
            canRegainStamina = false;
            StartCoroutine(RegainStamina());
        }

        if (Input.GetKey(KeyCode.LeftShift) && leftStamina > 0.0f) //Left shift is being held
        {
            rb.AddForce(2 * move);
            leftStamina -= Time.deltaTime;
            float currStaminaPct = leftStamina / maxTimeStamina;
            OnStaminaPctChanged(currStaminaPct);
        }
        else
        {
            rb.AddForce(move);

            if (canRegainStamina && leftStamina < maxTimeStamina)
            {
                leftStamina += Time.deltaTime * 0.5f;
                float currStaminaPct = leftStamina / maxTimeStamina;
                OnStaminaPctChanged(currStaminaPct);
            }
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            isOnGround = true;
        }
    }

    //consider when character is jumping .. it will exit collision.
    void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            isOnGround = false;
        }
    }

    IEnumerator RegainStamina()
    {
        yield return new WaitForSeconds(3.0f);
        canRegainStamina = true;
        leftStamina = maxTimeStamina * 0.1f;
        float currStaminaPct = leftStamina / maxTimeStamina;
        OnStaminaPctChanged(currStaminaPct);
    }
}
