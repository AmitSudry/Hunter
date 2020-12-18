using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    private GameObject player;
    public bool followPlayer = false;

    public Transform[] patrolPoints;
    private int pointIndex = 0;

    public Transform[] exitPoints;

    private bool reachedDest = false;
    public bool isActiveCreature = false;
    private int currExitPoint = -1;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(patrolPoints==null)
        {
            Debug.Log("No referenced patrol points");
            return;
        }
        if (exitPoints == null)
        {
            Debug.Log("No referenced exit points");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer)
        {   
            if(isActiveCreature)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                if (currExitPoint == -1)
                    currExitPoint = Random.Range(0, exitPoints.Length - 1);
                agent.SetDestination(exitPoints[currExitPoint].position);
                agent.speed *= 2;
            }

            if (ReachedDestination())
            {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("Lose");
            }
        }
        else
        {
            if(reachedDest)
            {
                if (++pointIndex == patrolPoints.Length)
                    pointIndex = 0;

                agent.SetDestination(patrolPoints[pointIndex].position);
                reachedDest = false;
            }
            else
            {
                if (ReachedDestination())
                    reachedDest = true;
            }
            
        }
    }

    bool ReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
