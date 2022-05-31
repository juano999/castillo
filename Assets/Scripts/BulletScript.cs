using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    private Vector2 Direction;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rigidbody2d.velocity = Direction * speed;
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }
}
