using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = player.position.y + 20.0f;
        transform.position = newPos;
    }
}
