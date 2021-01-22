using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //private Transform destination;
    //private Transfrom playerTrans = null;

    private Transform otherPortal = null;
    public bool isBlue = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider coll)
    {
        GameObject portal;
        
        if (isBlue)
            portal = GameObject.FindGameObjectWithTag("OrangePortal");
        else
            portal = GameObject.FindGameObjectWithTag("BluePortal");

        if(portal == null)
        {
            Debug.Log("Portal is missing!");
            return;
        }

        otherPortal = portal.GetComponent<Transform>();

        coll.transform.position = new Vector3(otherPortal.position.x + 3, otherPortal.position.y, otherPortal.position.z + 3);
    }
}
