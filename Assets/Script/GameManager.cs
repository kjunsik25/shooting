using System.Data;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    public float maxSpawnDelay;
    public float curSpwnDelay;

    public Text scoreText;
    public Image[] lifeimage;
    public Image[] boomimage;
    public GameObject gameOverSet;
    public GameObject player;
    void Update()
    {
        curSpwnDelay += Time.deltaTime;

        if(curSpwnDelay > maxSpawnDelay){
            spawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpwnDelay = 0;
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score); 
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
    public void UpdateLifeIcon(int life)
    {
        for(int index=0; index<3; index++){
            lifeimage[index].color = new Color(1,1,1,0);
        }
        for(int index=0; index<life; index++){
            lifeimage[index].color = new Color(1,1,1,1);
        }
    }

    public void UpdateBoomIcon(int life)
    {
        for(int index=0; index<3; index++){
            boomimage[index].color = new Color(1,1,1,0);
        }
        for(int index=0; index<life; index++){
            boomimage[index].color = new Color(1,1,1,1);
        }
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }
     public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }
    void RespawnPlayerExe()
    {
        player.transform.position= Vector3.down * 3.5f;
        player.SetActive(true);

        Player palyerlogic = player.GetComponent<Player>();
        palyerlogic.isHit= false;
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
