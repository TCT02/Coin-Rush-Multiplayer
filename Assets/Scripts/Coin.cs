using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.Netcode;
public class Coin : NetworkBehaviour
{
    // Start is called before the first frame update
    public float healVal;
    private float rotateVal;
    public NetworkVariable<Vector3> currPos = new NetworkVariable<Vector3>();

    void Start()
    {    
        healVal = 50;
        rotateVal = 0;
        updatePosServerRpc(transform.position);
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
            /*
            //Heal player a certain amount of HP or max their hp out if they're near the limit
            float currHP = collidedWith.GetComponent<Player>().currHP.Value;
            float maxHP = collidedWith.GetComponent<Player>().maxHP.Value;
            if (currHP + healVal > maxHP)
            {
                collidedWith.GetComponent<Player>().currHP.Value = maxHP;
                //convert to update serverside
                //overHealServerRpc(collidedWith, currHP, maxHP);
            }
            else
            {
                collidedWith.GetComponent<Player>().currHP.Value += healVal;
                //convert to update serverside
                //healServerRpc(collidedWith, currHP, maxHP);
            }
            */
            //Award points to the UI


            //Destroy Coin
            if (IsServer)
            {
                //Destroy(gameObject);
                NetworkObject.Despawn(true);
            }          
        }
    }

   
    [Rpc(SendTo.Server)]
    void updatePosServerRpc(Vector3 currP)
    {
        currPos.Value = currP;
        //transform.position = currP;
    }
    /*
   [Rpc(SendTo.Server)]
   void overHealServerRpc(Collider coll, float currHP, float maxHP)
   {
       coll.GetComponent<Player>().currHP.Value = maxHP;
       //transform.position = currP;
   }
   void healServerRpc(Collider coll, float currHP, float maxHP)
   {
       coll.GetComponent<Player>().currHP.Value += healVal;
       //transform.position = currP;
   }
   */


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
