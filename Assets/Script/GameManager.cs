using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    public float maxSpawnDelay;
    public float curSpwnDelay;

    void Update()
    {
        curSpwnDelay += Time.deltaTime;

        if(curSpwnDelay > maxSpawnDelay){
            spawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpwnDelay = 0;
        }
    }

    void spawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);
        Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);   
    }
}
