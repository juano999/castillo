using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Collections;
using TMPro;
using System;

public class PlayerMovement : NetworkBehaviour

{
    public MatchManagerScript matchManager;
    public NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes>();

    [SerializeField]
    public NetworkVariable<int> Health = new NetworkVariable<int>(20, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField]
    public NetworkVariable<bool> IsDead = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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



            if (Health.Value == 0)
            {
                IsDead.Value = true;
                Invoke(nameof(Respawn), 3);
            }

        }





    }

    public void Respawn()
    {

        Health.Value = 20;
        Vector3 refPosition;
        if (gameObject.name == "Player1(Clone)")
        {

            refPosition = GameObject.Find("Player2(Clone)").transform.position;
            
        }
        else if (gameObject.name == "Player2(Clone)")
        {
            refPosition = GameObject.Find("Player1(Clone)").transform.position;
            
        }
        else
        {
            Debug.Log("No se encontro ningun player");
            refPosition = new Vector3(0, 0, 0);
        }

        transform.position = new Vector3(0, refPosition.y, 0);
        IsDead.Value = false;
       

    }

    public override void OnNetworkSpawn()
    {
        IsDead.OnValueChanged += IsDeadChanged;
        matchManager = GameObject.Find("MatchManager").GetComponent<MatchManagerScript>();
    }


    public void IsDeadChanged(bool previous, bool current)
    {
        if (IsDead.Value)
        {
            gameObject.SetActive(false);
            
            Debug.Log("Ha muerto el jugador: " + OwnerClientId+1);
            matchManager.ChangePlayerWithAdvantageServerRpc(Convert.ToInt32(OwnerClientId));
            
        } else
        {
            gameObject.SetActive(true);
        }
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
        if (!IsOwner) RequestTakeDamageServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestTakeDamageServerRpc()
    {
        TakeDamageClientRpc();
    }

    [ClientRpc]
    private void TakeDamageClientRpc()
    {
        if (IsOwner) Health.Value--;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("objeto collision" + collision.gameObject.name);
    }





}
