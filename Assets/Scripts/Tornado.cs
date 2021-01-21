using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tornado : MonoBehaviour
{
    public Transform tornadoCenterPull;

    public float pullForce = 500.0f;
    public float refreshRate;

    private float radius;

    public float wanderRadius;
    public float wanderTimer;

    private NavMeshAgent agent;
    private float timer = 0.0f;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        radius = gameObject.GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer || ReachedDestination())
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0.0f;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Player")
        {
            if(other == null)
            {
                Debug.Log("Exiting3");
                return;
            }

            StartCoroutine(pullObject(other, true));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == null)
        {
            Debug.Log("Exiting2");
            return;
        }

        StartCoroutine(pullObject(other, false));
    }

    IEnumerator pullObject(Collider x, bool pull)
    {
        if (x == null)
        {
            Debug.Log("Exiting1");
            yield break;
        }
            

        if (pull)
        {
            Vector3 forceDir = tornadoCenterPull.position - x.transform.position;
            float fraction = Vector3.Distance(transform.position, x.transform.position) / radius;
            if (fraction > 1)
                fraction = 1;
            float force = (1 - fraction) * pullForce; //The inner your position the greater the pull

            x.GetComponent<Rigidbody>().AddForce(forceDir.normalized * force * Time.deltaTime);
            yield return null;
            StartCoroutine(pullObject(x, true));
        }
    }

    bool ReachedDestination()
    {
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
