using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    Vector3 initPos;

    bool canTouch = false;
    bool promptActive = false;

    [SerializeField] TextMeshPro promptText;

    //On Screen exit button
    public void OnExit() //When the exit button is pressed.
    {
        //print("Exiting app");
        //Application.Quit();

    }

    // Start is called before the first frame update
    void Start()
    {
        canTouch = false;

        promptActive = false;

        initPos = gameObject.transform.position;
        gameObject.transform.position += new Vector3(0,20,0);
        //StartCoroutine(titleBobbing());
        //Invoke("showPrompt", 5);
    }

    // Update is called once per frame
    void Update()
    {    
        if (Input.touchCount > 0 && canTouch == true)
        {
            Touch touch;

            if (Input.touchCount == 1) //1 finger input
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) //if tapping
                {
                    print("Loading Game");
                    SceneManager.LoadScene("Game Map");
                }
                else if (touch.phase == TouchPhase.Stationary) //if holding
                {
                    //print("Exiting app from 1 touch");
                    //Application.Quit();
                }
            }
            if (Input.touchCount == 2) //2 finger input
            {
                touch = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Stationary) //if holding
                {
                    print("Exiting app");
                    Application.Quit();
                }
            }

        }
        

    }
  
    private void FixedUpdate()
    {
        //initPos + (new Vector3(0, 5, 0))
        //new Vector3(initPos.x, initPos.y + 5, initPos.z)

        if (gameObject.transform.position.y > initPos.y)
        {
            transform.Translate(new Vector3(0, -1, 0) * 10 * Time.deltaTime);
        }
        else if (gameObject.transform.position.y <= initPos.y && promptActive == false)
        {
            showPrompt();
        }
     
    }

    void showPrompt ()
    {
        promptText.enabled = true;
        promptActive = true;
        canTouch = true;
    }
    
    /*
    IEnumerator titleBobbing()
    {
        while(true)
        {
            transform.Translate(new Vector3(0, 10, 0) * 2 * Time.deltaTime);
            yield return new WaitForSeconds(1);
            transform.Translate(new Vector3(0, 10, 0) * 2 * Time.deltaTime);
            yield return new WaitForSeconds(1);
        }

    }
    */

}
