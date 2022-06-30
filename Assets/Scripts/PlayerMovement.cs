using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Collections;

public class PlayerMovement : NetworkBehaviour

{

    public NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes>();
    
    public NetworkVariable<int> Health = new NetworkVariable<int>(100);

    
    public GameObject InGamePanel;
    float horizontalMove = 0;
    float verticalMove = 0;

    public float runSpeedHorizontal = 3;
    public float runSpeedVertical = 3;
    public float runSpeed = 2;
    

    Rigidbody2D Rigidbody2D;
    public Joystick joystick;
    
    public Button jumpBtn;
    

    private bool grounded;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        
        jumpBtn.onClick.AddListener(Jump);
        if(!IsOwner)
        {
            InGamePanel.SetActive(false);
        }
        Health.Value = 100;

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) Destroy(this);
        if (IsOwner)
        {


            //Movimiento horizontal
            horizontalMove = joystick.Horizontal * runSpeedHorizontal;
            transform.position += new Vector3(horizontalMove, verticalMove, 0) * Time.deltaTime * runSpeed;

            if (horizontalMove < 0.0f) transform.localScale = new Vector3(-0.12f, 0.12f, 0.12f);
            else if (horizontalMove > 0.0f) transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);

            // Se activa la animacion cuando horizontalMove sea diferente de 0
            animator.SetBool("running", horizontalMove != 0.0f);

            Vector3 auxVector = new Vector3(0, -0.35f, 0);
            // Nos aseguramos que este en el suelo para saltar 
            Debug.DrawRay(transform.position + auxVector, Vector3.down * 0.1f, Color.red);
            if (Physics2D.Raycast(transform.position + auxVector, Vector3.down, 0.1f))
            {
                grounded = true;
            }
            else grounded = false;

            //Debug.Log(transform.position);
            
        }

    }

    public void Jump()
    {  
        if (grounded)
        {
            Rigidbody2D.velocity = Vector2.up * runSpeedVertical;
        }
    }

    

   
}
