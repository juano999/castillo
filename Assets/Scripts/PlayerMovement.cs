using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Collections;
using TMPro;

public class PlayerMovement : NetworkBehaviour

{
    [SerializeField]
    public const int MAX_HEALTH = 20;

    public NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes>();

    [SerializeField]
    public NetworkVariable<int> Health = new NetworkVariable<int>(MAX_HEALTH, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public TMP_Text TotalLifeText;
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

        if (!IsOwner) InGamePanel.SetActive(false);

        TotalLifeText.text = "100/100";
    }

    // Update is called once per frame
    void Update()
    {
        //if (!IsOwner) Destroy(this);
        TotalLifeText.text = $"{Health.Value}/100";

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

            if (Health.Value == 0) Invoke(nameof(Respawn), 3);


        }

        //Desactivar al jugador - Ilusion de que muere
        if (Health.Value > 0) gameObject.SetActive(true);
        else gameObject.SetActive(false);


    }

    public void Respawn()
    {
        Health.Value = MAX_HEALTH;
        Vector3 refPosition;
        if (gameObject.name == "Player1(Clone)")
        {

            refPosition = GameObject.Find("Player2(Clone)").transform.position;
            Debug.Log(" player1: " + GameObject.Find("Player1(Clone)").name);
        }
        else if (gameObject.name == "Player2(Clone)")
        {
            refPosition = GameObject.Find("Player1(Clone)").transform.position;
            Debug.Log(" player1: " + GameObject.Find("Player1(Clone)").name);
        }
        else
        {
            refPosition = new Vector3(0, 0, 0);
        }
        Debug.Log("refPosition: " + refPosition);
        transform.position = new Vector3(0, refPosition.y, 0);

    }

    public void Jump()
    {
        if (grounded)
        {
            Rigidbody2D.velocity = Vector2.up * runSpeedVertical;
        }
    }

    public void Hit()
    {
        //Debug.Log("isOwner"+ IsOwner);
        //Health.Value = Health.Value - 1;
        //TotalLifeText.text = $"{Health.Value} /100";
        if (IsOwner)
        {
            Debug.Log("Proyectil Impactado");
            //TotalLifeText.text = $"{Health.Value}/100";
        }
        else
        {
            RequestTakeDamageServerRpc();
            //TotalLifeText.text = $"{Health.Value}/100";
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestTakeDamageServerRpc()
    {
        Debug.Log("Server?" + IsServer);
        TakeDamageClientRpc();
    }

    [ClientRpc]
    private void TakeDamageClientRpc()
    {
        if (IsOwner)
        {
            Health.Value--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("objeto collision" + collision.gameObject.name);
    }





}
