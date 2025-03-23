using UnityEngine;

public class Enemy : MonoBehaviour
{
   public float Speed;
   public int Health;
   public Sprite[] sprites;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.linearVelocity = Vector2.down * Speed;
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
            Bullet Playerbullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(Playerbullet.damge);

            Destroy(collision.gameObject);
        }
        
    }
}
