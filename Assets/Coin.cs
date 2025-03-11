using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    private float healVal;
    private float rotateVal;

    void Start()
    {    
        healVal = 50;
        rotateVal = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotateVal += 2f;
        //gameObject.transform.rotation = Quaternion.Euler(0, rotateVal,0);
        gameObject.transform.rotation = Quaternion.Euler(rotateVal, 0, 0);
        //gameObject.transform.rotation = Quaternion.Euler(0, 0, rotateVal);
    }

    
    private void OnTriggerEnter(Collider collidedWith)
    {
        //GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Player"))
        {
            //Heal player a certain amount of HP or max their hp out if they're near the limit
            float currHP = collidedWith.GetComponent<Player>().currHP;
            float maxHP = collidedWith.GetComponent<Player>().maxHP;
            if (currHP + healVal > maxHP)
            {
                collidedWith.GetComponent<Player>().currHP = maxHP;
            }
            else
            {
                collidedWith.GetComponent<Player>().currHP += healVal;
            }

            //Award points to the UI


            //Destroy Coin
            Destroy(gameObject);

        }
    }
    

    /*
    private void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Plr"))
        {
            float currHP = collidedWith.GetComponent<Player>().currHP;
            float maxHP = collidedWith.GetComponent<Player>().maxHP;
            if (currHP + healVal > maxHP)
            {
                collidedWith.GetComponent<Player>().currHP = maxHP;
            }
            else
            {
                collidedWith.GetComponent<Player>().currHP += healVal;
            }
            Destroy(gameObject);

        }
    }
    */
}
