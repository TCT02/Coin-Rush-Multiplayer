using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
//using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.CullingGroup;

public class Player : NetworkBehaviour
{
    //Player Status
    public NetworkVariable<float> currHP = new NetworkVariable<float>(100);
    public NetworkVariable<float> maxHP = new NetworkVariable<float>(100);
    public NetworkVariable<int> numCoins = new NetworkVariable<int>(0);
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> onGround = new NetworkVariable<bool>(true);
    //public NetworkVariable<Renderer> plrColor = new NetworkVariable<Renderer>();


    string localInput;

    [SerializeField] bool isMobileClient = false;

    //Physics
    Rigidbody rb;

    //Sound
    [SerializeField] AudioClip coinPickup;
    [SerializeField] AudioClip jump;

    AudioSource sound;

    //Controls
    [SerializeField] private InputActionReference moveAction;

    [SerializeField] float speed = 8;
    [SerializeField] float jumpPower = 200;

    

    //Jump functions
    public void OnJump() //When the jump button is pressed for mobile.
    {
            if (onGround.Value == true && isMobileClient == true)
            {
                print("You just jumped");
                sound.clip = jump;
                sound.Play();
            rb.AddForce(new Vector3(0, jumpPower, 0));
            }
            else
            {
                print("Cannot jump, you are in the air");
            }
        
    }

    public void keyboardJump() //When space is pressed for PC controls.
    {
        if (onGround.Value == true)
        {
            print("You just jumped");
            sound.clip = jump;
            sound.Play();
            rb.AddForce(new Vector3(0, jumpPower, 0));
        }
        else
        {
            print("Cannot jump, you are in the air");
        }
    }

    public void OnExit() //When the exit button is pressed.
    {
        print("Returning to Main Menu");
        SceneManager.LoadScene("Main Menu");

    }


    // Start is called before the first frame update
    void Start()
    {

        //Sound initialization
        sound = GetComponent<AudioSource>();
        sound.clip = coinPickup;

        //Control initialization
        rb = gameObject.GetComponent<Rigidbody>();

        //currHP = 100;
        //maxHP = 100;
        //numCoins = 0;
        //isDead.Value = false;

        if (IsClient) 
        {
            //updateColorServerRpc(Color.blue);
        }
        else if (IsServer)
        {
            //updateColorServerRpc(Color.red);
        }

        //Bleed damage over time
        //StartCoroutine("dmgOverTime");

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            return;
        }
       
        if (Input.GetKey(KeyCode.W))
        {
            localInput = "w";
        }
        if (Input.GetKey(KeyCode.A))
        {
            localInput = "a";
        }
        if (Input.GetKey(KeyCode.S))
        {
            localInput = "s";
        }
        if (Input.GetKey(KeyCode.D))
        {
            localInput = "d";
        }
        if (Input.GetKeyDown(KeyCode.Space) && onGround.Value == true)
        {
            localInput = " ";
        }
        
        //print(localInput);

        movementForServerRpc(localInput);
        localInput = null;

    }

    private void moveTransform(Vector3 dir)
    { 
        transform.Translate(dir);
    }

    [Rpc(SendTo.Server)]
    void movementForServerRpc(string localIn)
    {
        //Player Status
        if (currHP.Value <= 0) //If HP is 0, declare the player as dead
        {
            isDead.Value=true;
        }
        else //Otherwise, allow them to move.
        {

            if (isMobileClient == true) //Use mobile controls
            {
                //Mobile Movement and Controls
                //Movedirection uses X and Y like in a 2D game so it needs to be converted to 3D X and Z
                Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
                moveTransform((new Vector3(moveDirection.x, 0, moveDirection.y)) * speed * Time.deltaTime);
            }
            else //Use keyboard controls
            {
                //Keyboard Controls
                //Vector2 moveDirection;
                //if (Input.GetKey(KeyCode.W))
                if (localIn == "w")
                {
                    //transform.Translate((new Vector3(0, 0, 1)) * speed * Time.deltaTime);                 

                    moveTransform((new Vector3(0, 0, 1)) * speed * Time.deltaTime);
                }
                //if (Input.GetKey(KeyCode.A))
                if (localIn == "a")
                {
                    //transform.Translate((new Vector3(-1, 0, 0)) * speed * Time.deltaTime);
                    moveTransform((new Vector3(-1, 0, 0)) * speed * Time.deltaTime);
                }
                //if (Input.GetKey(KeyCode.S))
                if (localIn == "s")
                {
                    //transform.Translate((new Vector3(0, 0, -1)) * speed * Time.deltaTime);
                    moveTransform((new Vector3(0, 0, -1)) * speed * Time.deltaTime);
                }
                //if (Input.GetKey(KeyCode.D))
                if (localIn == "d")
                {
                    //transform.Translate((new Vector3(1, 0, 0)) * speed * Time.deltaTime);
                    moveTransform((new Vector3(1, 0, 0)) * speed * Time.deltaTime);

                }
                //if (Input.GetKeyDown(KeyCode.Space) && onGround.Value == true)
                if (localIn == " " && onGround.Value == true)
                {
                    keyboardJump();
                }

            }
        }
    }


    public IEnumerator dmgOverTime()
    {
        while (currHP.Value > 0)
        {
            //currHP -= 10;
            updateHPServerRpc(10);
            yield return new WaitForSeconds(1f);  // wait 1 second

        }

    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Ground"))
        {
            onGround.Value = true;

        }
    }

    private void OnCollisionExit(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Ground"))
        {
            onGround.Value = false;
        }

    }

    private void OnTriggerEnter(Collider collidedWith)
    {

        if (collidedWith.CompareTag("Coin"))
        {
            sound.clip = coinPickup;
            sound.Play();

            //numCoins++;
            updateCoinServerRpc(collidedWith.GetComponent<Coin>().healVal);
            

        }
    }

    //Server functions
    [Rpc(SendTo.Server)]
    void updateIsDeadServerRpc(bool status)
    {
        isDead.Value = status;
    }
    [Rpc(SendTo.Server)]
    void updateCoinServerRpc(float healVal)
    {
        numCoins.Value++;

        //Heal player a certain amount of HP or max their hp out if they're near the limit
        if (currHP.Value + healVal > maxHP.Value)
        {
            currHP.Value = maxHP.Value;
        }
        else
        {
            currHP.Value += healVal;

        }
    }

    [Rpc(SendTo.Server)]
    void updateHPServerRpc(float dmg)
    {
        currHP.Value -= dmg;
    }

    [Rpc(SendTo.Server)]
    void updateColorServerRpc(Color plrColor)
    {
        gameObject.GetComponent<Renderer>().material.color = plrColor;
    }

}
