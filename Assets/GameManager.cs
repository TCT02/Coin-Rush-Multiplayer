using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject coin;
    [SerializeField] GameObject playerChar;
    [SerializeField] GameObject playSpace;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] ScoreSave scoreSave;

    bool coinExists = true;
    Vector2 testPos;

    void Start()
    {
        playerChar = GameObject.Find("Player");
        playSpace = GameObject.Find("Ground");

    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerChar.GetComponent<Player>().isDead == false)
        {

            healthText.text = "HP: [" + playerChar.GetComponent<Player>().currHP + "|" + playerChar.GetComponent<Player>().maxHP + "]";
            coinText.text = "Coins: [" + playerChar.GetComponent<Player>().numCoins +"]";
        }
        else //When the player is dead do something to end the game and such
        {
            //Destroy(playerChar);

            scoreSave.coinScore = playerChar.GetComponent<Player>().numCoins;
            SceneManager.LoadScene("Game Over");
        }

    }

    private void FixedUpdate()
    {
        //if a coin is not found on the map, spawn a new one.
        print(GameObject.Find("Coin") == null && GameObject.Find("Coin(Clone)") == null);
        if (GameObject.Find("Coin") == null && GameObject.Find("Coin(Clone)") == null)
        { 
            spawnCoin();

        }
    }

    private void spawnCoin()
    {
        //If there are no coins detected, spawn a new one at a random location.
        //Spawn a coin below the map
        float edgeOffset = 6;
        float randomXPos; 
        float randomZPos;
        Vector3 spawnLoc;

        void randomPosGen()
        {
            randomXPos = UnityEngine.Random.Range(-playSpace.transform.localScale.x / 2, playSpace.transform.localScale.x / 2);
            randomZPos = UnityEngine.Random.Range(-playSpace.transform.localScale.z / 2, playSpace.transform.localScale.z / 2);
        }
        randomPosGen();

        if (randomXPos < 0)
        {
            randomXPos += edgeOffset;
        }
        else
        {
            randomXPos -= edgeOffset;
        }
        if (randomZPos < 0)
        {
            randomZPos += edgeOffset;
        }
        else
        {
            randomZPos -= edgeOffset;
        }
        
        spawnLoc = playSpace.transform.position + new Vector3(randomXPos, .75f, randomZPos);
        

        Instantiate(coin, spawnLoc, Quaternion.Euler(0, 0, 0));
        GameObject theCoin = GameObject.Find("Coin(Clone)");
        theCoin.transform.position = spawnLoc;

    }
}
