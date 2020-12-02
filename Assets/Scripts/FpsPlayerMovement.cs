using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsPlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Rigidbody rb;
    public float speed = 12.0f;
    public float upForce = 10000.0f;
    public bool isOnGround = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKey("q") && isOnGround)
        {

            rb.AddForce(0, upForce * Time.deltaTime, 0);
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
}
