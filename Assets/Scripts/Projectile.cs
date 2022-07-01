using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector3 _dir;
    PlayerMovement player;

    public void Init(Vector3 dir)
    {
        rb2d = GetComponent<Rigidbody2D>();
        //rb2d.velocity = dir;
        rb2d.AddForce(dir);
        Debug.Log("velocity: " + rb2d.velocity);
        Invoke(nameof(DestroyBall), 3);
    }

    
    private void DestroyBall()
    {

        //Instantiate(_particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        Debug.Log("player collision" + collision.gameObject.name);
        if (player != null)
        {
            player.Hit();
        }
        DestroyBall();
    }
}