using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    public float maxSpawnDelay;
    public float curSpwnDelay;

    public GameObject player;
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
        GameObject enemy = Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);   

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if(ranPoint == 5 || ranPoint == 6 || ranPoint == 8){
            enemy.transform.Rotate(Vector3.back*90);
            rigid.linearVelocity = new Vector2(enemyLogic.Speed, -1);
        }
        else if(ranPoint == 7 || ranPoint == 9){
            enemy.transform.Rotate(Vector3.forward*90);
            rigid.linearVelocity = new Vector2(enemyLogic.Speed * (-1), -1);
        }
        else {
            rigid.linearVelocity = new Vector2(0, enemyLogic.Speed * (-1));
        }
    }
     public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }
    void RespawnPlayerExe()
    {
        player.transform.position= Vector3.down * 3.5f;
        player.SetActive(true);
    }
}
