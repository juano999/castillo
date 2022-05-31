using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour

{

    public GameObject BulletPrefab;
    float horizontalMove = 0;
    float verticalMove = 0;

    public float runSpeedHorizontal = 3;
    public float runSpeedVertical = 3;
    public float runSpeed = 0;
    //public float jumpForce;

    Rigidbody2D rigidbody2D;
    public Joystick joystick;
    public Button jumpBtn;
    public Button shootBtn;

    private bool grounded;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpBtn.onClick.AddListener(Jump);
        shootBtn.onClick.AddListener(Shoot);
    }

    // Update is called once per frame
    void Update()
    {
        //verticalMove = joystick.Vertical * runSpeedVertical;
        //Movimiento horizontal
        horizontalMove = joystick.Horizontal * runSpeedHorizontal;
        transform.position += new Vector3(horizontalMove, 0 , 0) * Time.deltaTime * runSpeed;

        if (horizontalMove < 0.0f) transform.localScale = new Vector3(-0.12f, 0.12f, 0.12f);
        else if (horizontalMove > 0.0f) transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);

        // Se activa la animacion cuando horizontalMove sea diferente de 0
        animator.SetBool("running", horizontalMove != 0.0f);


        // Nos aseguramos que este en el suelo para saltar 
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            grounded = true;
        }
        else grounded = false;

        

        
    }

    private void Jump()
    {
        if (grounded)
        {

        horizontalMove = joystick.Horizontal * runSpeedHorizontal;
        verticalMove = joystick.Vertical * runSpeedVertical;
        transform.position += new Vector3(horizontalMove, verticalMove, 0) * Time.deltaTime * runSpeed;
        }
    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x > 0.0f) direction = Vector2.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);

    }
}
