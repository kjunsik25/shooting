using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damge;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet"){
            Destroy(gameObject);
        }

    }
}
