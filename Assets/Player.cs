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

    public void OnJump() //When the jump button is pressed.
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
            //Movement and Controls
            //Movedirection uses X and Y like in a 2D game so it needs to be converted to 3D X and Z
            Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
            transform.Translate((new Vector3(moveDirection.x, 0, moveDirection.y)) * speed * Time.deltaTime);
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
