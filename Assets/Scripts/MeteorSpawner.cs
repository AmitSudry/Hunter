using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public Transform[] meteors;

    public float minx = 0.0f;
    public float maxx = 0.0f;
    public float minz = 0.0f;
    public float maxz = 0.0f;

    private float delta = 0.0f;
    public float spawnEvery = 3.0f;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (meteors.Length == 0)
        {
            Debug.Log("No referenced meteors");
            return;
        }
        delta = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(delta >= spawnEvery)
        {
            delta = 0.0f;
            spawnMeteor();
            return;
        }

        delta += Time.deltaTime;
    }

    void spawnMeteor()
    {
        int index = Random.Range(0, meteors.Length);
        Vector3 pos = new Vector3(Random.Range(minx, maxx), 300.0f, Random.Range(minz, maxz));

        Instantiate(meteors[index], pos, Quaternion.identity);
    }
}
