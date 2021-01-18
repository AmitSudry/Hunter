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

    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Jump");
        
        Vector3 move = transform.right * x * sideForce + transform.forward * z * forwardForce;
        Vector3 jump = transform.up * y * upForce;
        /*
        if (Input.GetKey("w"))
        {
            rb.AddForce(-forwardForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("s"))
        {
            rb.AddForce(forwardForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(0, 0, sideForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKey("a"))
        {
            rb.AddForce(0, 0, -sideForce * Time.deltaTime, ForceMode.VelocityChange);
        }
       
        */
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(jump);
        }
        rb.AddForce(move);
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
}
