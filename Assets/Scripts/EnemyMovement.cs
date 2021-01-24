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

    private bool first = true; //Update agent speed only at the first time

    public bool isBossCreature = false;

    public bool isZigZag = true;

    private float timer = 0.0f;
    public float threshold = 0.25f;

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
        if (followPlayer) //Player is exposed
        {   
            if(isActiveCreature)
            {
                timer += Time.deltaTime;

                if (first)
                {
                    agent.speed *= 2.0f;
                    first = false;
                    agent.SetDestination(player.transform.position);
                }

                if (timer > threshold)
                {
                    agent.SetDestination(player.transform.position);
                    timer = 0.0f;
                }
            }
            else
            {
                if (first)
                {
                    currExitPoint = Random.Range(0, exitPoints.Length);
                    agent.SetDestination(exitPoints[currExitPoint].position);
                    agent.speed *= 1.5f;
                    first = false;
                }
            }

            if (agent.speed > 12)
                agent.speed = 12;

            if (ReachedDestination())
            {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("Lose");
            }
        }
        else
        {
            if (Vector3.Distance(player.transform.position, agent.transform.position) <= 20.0f) //Dont get too close to the enemy
                followPlayer = true;

            if (reachedDest) //Reached patrol point
            {
                int prevPoint = pointIndex;
                do
                {
                    pointIndex = Random.Range(0, patrolPoints.Length);
                }
                while (prevPoint == pointIndex);

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
        if(isBossCreature)
        {
            if (Vector3.Distance(player.transform.position, agent.transform.position) <= 10.0f) //Dont get too close to the enemy
                return true;
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude <= 50.0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
