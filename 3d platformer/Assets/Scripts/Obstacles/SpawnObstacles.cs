using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public GameObject Ball;
    public Transform SpawnLocation;
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
        yield return new WaitForSeconds(2f);
        canSpawn = true;
        GameObject temp = Instantiate(Ball, SpawnLocation.position, Quaternion.identity);
        Destroy(temp, 8f);
    }
}
