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

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w"))
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
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(0, upForce * Time.deltaTime, 0);
        }

        

    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.name == "Ground")
        {
            isOnGround = true;
        }
    }

    //consider when character is jumping .. it will exit collision.
    void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.name == "Ground")
        {
            isOnGround = false;
        }
    }
}
