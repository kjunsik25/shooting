using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float power;
    public float maxPower;
    public int boom;
    public int maxBoom;
    public float maxShotDelay;
    public float curShotDelay;
    public int life;
    public int score;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;
    public GameObject bulletObjA;  
    public GameObject bulletObjB;
    public GameManager manager;
    public GameObject boomEffect;
    public bool isHit;
    public bool isBoomTime;

    Animator anim;

    void Awake()
    {
        anim= GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      Move();
      Fire();
      Boom();
      Reload();
    }

    void Move()// 방향키를 눌렀을때 움직이게 하는 함수
    {
        float h = Input.GetAxisRaw("Horizontal");
        if((isTouchRight && h == 1 ) || (isTouchLeft && h == -1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if((isTouchTop && v == 1 ) || (isTouchBottom && v == -1))
            v = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * Speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")){
            anim.SetInteger("Input", (int)h);
        }
    }

    void Fire() // 투사체 발사 함수
    {
        if(!Input.GetKey(KeyCode.Space)){
            return;
        }
        if(curShotDelay < maxShotDelay){
            return;
        }

        switch(power){
            case 0:
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.up*10, ForceMode2D.Impulse);
            break;
            case 1:
            GameObject bulletR1 = Instantiate(bulletObjA, transform.position+ Vector3.right*0.1f, transform.rotation);
            GameObject bulletL1 = Instantiate(bulletObjA, transform.position+ Vector3.left*0.1f, transform.rotation);
            Rigidbody2D rigidR1 = bulletR1.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL1 = bulletL1.GetComponent<Rigidbody2D>();
            rigidR1.AddForce(Vector2.up*10, ForceMode2D.Impulse);
            rigidL1.AddForce(Vector2.up*10, ForceMode2D.Impulse);
            break;
            case 2:
            GameObject bulletR2 = Instantiate(bulletObjA, transform.position+ Vector3.right*0.3f, transform.rotation);
            GameObject bulletL2 = Instantiate(bulletObjA, transform.position+ Vector3.left*0.3f, transform.rotation);
            GameObject bulletC2 = Instantiate(bulletObjB, transform.position, transform.rotation);
            Rigidbody2D rigidR2 = bulletR2.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL2= bulletL2.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidC2= bulletC2.GetComponent<Rigidbody2D>();
            rigidR2.AddForce(Vector2.up*10, ForceMode2D.Impulse);
            rigidL2.AddForce(Vector2.up*10, ForceMode2D.Impulse);
            rigidC2.AddForce(Vector2.up*10, ForceMode2D.Impulse);
            break;
        }
        

        curShotDelay = 0;
    }

    void Reload() //재장전 함수 
    {
        curShotDelay += Time.deltaTime;
    }

    void Boom()
    {
        if (!Input.GetKey(KeyCode.Q))
            return;

        if(isBoomTime)
            return;
        if(boom == 0)
            return;

        boom--;
        isBoomTime = true;
        manager.UpdateBoomIcon(boom);

        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 2f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int index=0; index < enemies.Length; index++){
            Enemy enemyLogic = enemies[index].GetComponent<Enemy>();
            enemyLogic.OnHit(1000);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for(int index=0; index < bullets.Length; index++){
            Destroy(bullets[index]);
        }
        
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border"){
            switch(collision.gameObject.name){
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet"){
            if(isHit)
                return;
            isHit = true;
            life --;
            manager.UpdateLifeIcon(life);
            if(life == 0){
                manager.GameOver();
            }
            else{
                manager.RespawnPlayer();
            }
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Item"){
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type){
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if(power == maxPower)
                        score +=500;
                    else
                        power++;
                    break;
                case "Boom":
                    if(boom == maxBoom)
                        score +=500;
                    else{
                        boom++;
                        manager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            Destroy(collision.gameObject);
        }
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }    

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border"){
            switch(collision.gameObject.name){
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
