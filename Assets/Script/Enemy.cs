using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyname;
    public float Speed;
    public int Health;
    public Sprite[] sprites;

    public GameObject bulletObjA;  
    public GameObject bulletObjB;
    public GameObject player;

    public float maxShotDelay;
    public float curShotDelay;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

     void Update()
    {
      Fire();
      Reload();
    }

    void Fire() // 투사체 발사 함수
    {
    
        if(curShotDelay < maxShotDelay){
            return;
        }

        if (enemyname == "S"){
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized*5, ForceMode2D.Impulse);

        }
        else if (enemyname == "L"){
            GameObject bulletR = Instantiate(bulletObjB, transform.position + Vector3.right*0.3f, transform.rotation);
            GameObject bulletL = Instantiate(bulletObjB, transform.position + Vector3.left*0.3f, transform.rotation);
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right *0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidR.AddForce(dirVecR.normalized *5, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized *5, ForceMode2D.Impulse);

        }

        curShotDelay = 0;
    }

    void Reload() //재장전 함수 
    {
        curShotDelay += Time.deltaTime;
    }

    void OnHit(int damage) //데미지 입는 함수
    {
        Health -= damage;
        spriteRenderer.sprite = sprites[1]; 
        Invoke("ReturnSprite", 0.1f);

        if(Health <=0){
            Destroy(gameObject);
        }
    }

    void ReturnSprite(){
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet"){
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet"){
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damge);

            Destroy(collision.gameObject); 
        }
        
    }
}
