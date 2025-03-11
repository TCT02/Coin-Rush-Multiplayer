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

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerPrefab2;

    [SerializeField] GameObject playerChar;
    [SerializeField] GameObject playerChar2;
    [SerializeField] GameObject playSpace;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI healthTextP2;
    [SerializeField] TextMeshProUGUI coinTextP2;

    [SerializeField] ScoreSave scoreSave;
    //[SerializeField] ScoreSave scoreSaveP2;

    bool coinExists = true;
    Vector2 testPos;

    void Start()
    {
        playerChar = GameObject.Find("Player");
        playerChar2 = GameObject.Find("Player2");
        playSpace = GameObject.Find("Ground");

    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerChar.GetComponent<Player>().isDead == false && playerChar2.GetComponent<Player>().isDead == false)
        {
            //Update Player 1 HP and Coins
            healthText.text = "HP: [" + playerChar.GetComponent<Player>().currHP + "|" + playerChar.GetComponent<Player>().maxHP + "]";
            coinText.text = "Coins: [" + playerChar.GetComponent<Player>().numCoins +"]";

            //Update Player 1 HP and Coins
            healthTextP2.text = "HP: [" + playerChar2.GetComponent<Player>().currHP + "|" + playerChar2.GetComponent<Player>().maxHP + "]";
            coinTextP2.text = "Coins: [" + playerChar2.GetComponent<Player>().numCoins + "]";
        }
        else //When the player is dead do something to end the game and such
        {
            //Destroy(playerChar);
            //Player 1 and player 2 scores saved for persistence.
            scoreSave.coinScore = playerChar.GetComponent<Player>().numCoins;
            scoreSave.coinScore2 = playerChar2.GetComponent<Player>().numCoins;

            if (scoreSave.coinScore > scoreSave.coinScore2)
            { //Player 1 wins by score
                //scoreSave.playerColor = playerChar.GetComponent<Material>().color;
                setWinnerSave(Color.red, "Player 1 Wins", scoreSave.coinScore);
            }
            if (scoreSave.coinScore < scoreSave.coinScore2)
            { //Player 2 wins by score
                //scoreSave.playerColor = playerChar2.GetComponent<Material>().color;
                setWinnerSave(Color.blue, "Player 2 Wins", scoreSave.coinScore2);
            }
            else
            { // both players tied
                setWinnerSave(Color.magenta, "The game is a Tie", scoreSave.coinScore);
            }

            SceneManager.LoadScene("Game Over");
        }

    }

    private void setWinnerSave(Color plrColor, string plrWinner, int score)
    {
        scoreSave.playerColor = plrColor;
        scoreSave.winner = plrWinner;
        scoreSave.winnerScore = score;
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
