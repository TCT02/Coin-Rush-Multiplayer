using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Player Status
    public float currHP = 100;
    public float maxHP = 100;
    public int numCoins = 0;
    public bool isDead = false;
    public bool onGround = true;

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

    public void OnJump() //When the jump button is pressed for mobile.
    {
            if (onGround == true && isMobileClient == true)
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
        if (onGround == true)
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

        currHP = 100;
        maxHP = 100;
        numCoins = 0;
        isDead = false;

        //Bleed damage over time
        StartCoroutine("dmgOverTime");

    }

    // Update is called once per frame
    void Update()
    {
        //Player Status
        if (currHP <= 0) //If HP is 0, declare the player as dead
        {
            isDead = true;
        }
        else //Otherwise, allow them to move.
        {
            
            if (isMobileClient == true) //Use mobile controls
            {
                //Mobile Movement and Controls
                //Movedirection uses X and Y like in a 2D game so it needs to be converted to 3D X and Z
                Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
                transform.Translate((new Vector3(moveDirection.x, 0, moveDirection.y)) * speed * Time.deltaTime);
            }
            else //Use keyboard controls
            {     
                //Keyboard Controls
                Vector2 moveDirection;
                if (Input.GetKey(KeyCode.W))
                {
                    transform.Translate((new Vector3(0, 0, 1)) * speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate((new Vector3(-1, 0, 0)) * speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.Translate((new Vector3(0, 0, -1)) * speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate((new Vector3(1, 0, 0)) * speed * Time.deltaTime);
                }
                if (Input.GetKeyDown(KeyCode.Space) && onGround == true)
                {
                    keyboardJump();
                }


                
            }           
        }

        

    }

    IEnumerator dmgOverTime()
    {
        while (currHP > 0)
        {
            currHP -= 10;
            yield return new WaitForSeconds(1f);  // wait 1 second

        }

    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Ground"))
        {
            onGround = true;

        }
    }

    private void OnCollisionExit(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Ground"))
        {
            onGround = false;
        }

    }

    private void OnTriggerEnter(Collider collidedWith)
    {

        if (collidedWith.CompareTag("Coin"))
        {
            sound.clip = coinPickup;
            sound.Play();

            numCoins++;

        }
    }


}
