using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float power;
    public float maxShotDelay;
    public float curShotDelay;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;
    public GameObject bulletObjA;  
    public GameObject bulletObjB;
    public GameManager manager;

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
        if(!Input.GetButton("Fire1")){
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
            manager.RespawnPlayer();
            gameObject.SetActive(false);
        }
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
