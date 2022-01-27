using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public GameObject Ball;
    public Transform[] SpawnLocations;
    private bool canSpawn = false;
    void Start()
    {
        SpawnBalls();
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawn)
            SpawnBalls();
    }

    private void SpawnBalls()
    {
        StartCoroutine(PauseThenSpawn());
    }

    IEnumerator PauseThenSpawn()
    {
        canSpawn = false;
        yield return new WaitForSeconds(4f);
        canSpawn = true;
        int pos = (int)Random.Range(0,SpawnLocations.Length);
        GameObject temp = Instantiate(Ball, SpawnLocations[pos].position, Quaternion.identity);
        Destroy(temp, 8f);
    }
}
