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

    public List<Vector3> zigZagPoints = new List<Vector3>();
    private int currZigZagPointIndex = 0;

    public float offset = 2.0f;
    public int pointsFactor = 3;

    private float offsetMin;
    private float offsetMax;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        if (patrolPoints == null)
        {
            Debug.Log("No referenced patrol points");
            return;
        }
        if (exitPoints == null)
        {
            Debug.Log("No referenced exit points");
            return;
        }

        offsetMin = offset - 0.5f;
        offsetMax = offset + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer) //Player is exposed
        {
            if (isActiveCreature) //attacking
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

                if (ReachedDestination())
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Lose");
                }
            }
            else //running away
            {
                if (first)
                {
                    currExitPoint = Random.Range(0, exitPoints.Length);
                    agent.SetDestination(exitPoints[currExitPoint].position);
                    agent.speed *= 1.5f;

                    if(isZigZag)
                    {
                        Vector2 endPath = new Vector2(exitPoints[currExitPoint].position.x, exitPoints[currExitPoint].position.z);
                        Vector2 startPath = new Vector2(agent.transform.position.x, agent.transform.position.z);
                        GetZigZagPath(startPath, endPath);

                        reachedDest = true;
                        Debug.Log("Count: " + zigZagPoints.Count + " index: " + currZigZagPointIndex);
                    }

                    first = false;
                }

                if(isZigZag)
                {
                    if (reachedDest) //reached zigZag point
                    {
                        if(currZigZagPointIndex == zigZagPoints.Count - 1) //REACHED FINAL DESTINATION
                        {
                            Cursor.lockState = CursorLockMode.None;
                            SceneManager.LoadScene("Lose");
                        }
                        else
                        {
                            agent.SetDestination(zigZagPoints[currZigZagPointIndex++]);
                            reachedDest = false;
                        }
                    }
                    else
                    {
                        if (ReachedDestination())
                            reachedDest = true;
                    }
                }
            }

            if (agent.speed > 12)
                agent.speed = 12;

        }
        else //patroling
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
        if (isBossCreature)
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

    void GetZigZagPath(Vector2 v1, Vector2 v2)
    {
	    float dx = v2.x - v1.x;
        float dy = v2.y - v1.y;

        List<Vector2> points = new List<Vector2>();

        float length = Vector2.Distance(v1, v2);
        int numberOfPoints = pointsFactor * (int)(Mathf.Log(length, 2));
        Vector2 direction = new Vector2(dx, dy).normalized;

        Vector2 n1 = new Vector2(-dy, dx).normalized;
        Vector2 n2 = new Vector2(dy, -dx).normalized;

        float fraction = length / numberOfPoints;

	    for (int i = 0; i<numberOfPoints; i++)
	    {
            //Debug.Log(i);
            Vector2 dir = fraction * (i + 1) * direction;
            Vector2 nextPoint, toAdd;

            float off = Random.Range(offsetMin, offsetMax);

            if (i % 2 == 0) //even
		    {
                nextPoint = v1 + dir; //point on the original line
                toAdd = nextPoint + n1 * off;
		    }
            else
            {
                nextPoint = v1 + dir; //point on the original line
                toAdd = nextPoint + n2 * off;
            }

            points.Add(toAdd);
        }

        points.Add(v2);

        zigZagPoints = new List<Vector3>();
        for (int i=0; i < numberOfPoints + 1; i++)
        {
            zigZagPoints.Add(new Vector3(points[i].x, 0.0f, points[i].y));
        }
    }
}
