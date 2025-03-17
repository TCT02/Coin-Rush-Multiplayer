
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject coin;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerPrefab2;

    //private NetworkVariable<GameObject> player1 = new NetworkVariable<GameObject>();
    //private NetworkVariable<GameObject> player2 = new NetworkVariable<GameObject>();
    private List<GameObject> playersList = new List<GameObject>();

    [SerializeField] GameObject playerChar;
    [SerializeField] GameObject playerChar2;
    [SerializeField] GameObject playSpace;
    [SerializeField] GameObject playerSpawn;
    [SerializeField] GameObject playerSpawn2;
    GameObject theCoin;

    [SerializeField] Color redCol;
    [SerializeField] Color blueCol;

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI healthTextP2;
    [SerializeField] TextMeshProUGUI coinTextP2;


    //[SerializeField] NetworkVariable<TextMeshProUGUI> healthText = new NetworkVariable<TextMeshProUGUI>();
    //[SerializeField] NetworkVariable<TextMeshProUGUI> coinText = new NetworkVariable<TextMeshProUGUI>();
    //[SerializeField] NetworkVariable<TextMeshProUGUI> healthTextP2 = new NetworkVariable<TextMeshProUGUI>();
    //[SerializeField] NetworkVariable<TextMeshProUGUI> coinTextP2 = new NetworkVariable<TextMeshProUGUI>();

    [SerializeField] ScoreSave scoreSave;
    bool gameActive = false;
    //[SerializeField] ScoreSave scoreSaveP2;

    //bool coinExists = true;

    void Start()
    {
        playSpace = GameObject.Find("Ground");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (!IsServer)
        //{
        //    return;
        //}
        //print("Checking for players");
        
        if (GameObject.Find("Player(Clone)") && gameActive == false) //if a clone is found, do...
        {
            if (GameObject.Find("Player") == null)
            { //if a Player object named Player does not exist, rename a clone             
                GameObject.Find("Player(Clone)").name = "Player";
                playerChar = GameObject.Find("Player");
                playerChar.GetComponent<Renderer>().material.color = redCol;
                print(playerChar);
                playerChar.transform.position = playerSpawn.transform.position + new Vector3(0, 3, 0);
            }
            else if (GameObject.Find("Player2") == null) //if 
            { //if a Player object named Player2 does not exist, rename a clone             
                GameObject.Find("Player(Clone)").name = "Player2";
                playerChar2 = GameObject.Find("Player2");
                playerChar2.GetComponent<Renderer>().material.color = blueCol;
                playerChar2.transform.position = playerSpawn2.transform.position + new Vector3(0, 3, 0);
                print(playerChar2);
            }
            else //if (playerChar != null && playerChar2 != null)
            {             
                //gameActive = true;
                //updateColorsRpc();
                //print("Game started: " + gameActive);
            }
            
        }

        if ((playerChar != null && playerChar2 != null) && gameActive == false)
        {
            print("Both players exist: " + playerChar != null && playerChar2 != null);
            print("Game started: " + gameActive);
            gameActive = true;
            if (IsServer)
            {
                StartCoroutine(playerChar.GetComponent<Player>().dmgOverTime());
                StartCoroutine(playerChar2.GetComponent<Player>().dmgOverTime());
            }           
        }
      
        if (gameActive == true)
        {
            if (playerChar.GetComponent<Player>().isDead.Value == false && playerChar2.GetComponent<Player>().isDead.Value == false)
            {
                //Update Player 1 HP and Coins
                healthText.text = "HP: [" + playerChar.GetComponent<Player>().currHP.Value + "|" + playerChar.GetComponent<Player>().maxHP.Value + "]";
                coinText.text = "Coins: [" + playerChar.GetComponent<Player>().numCoins.Value + "]";

                //Update Player 1 HP and Coins
                healthTextP2.text = "HP: [" + playerChar2.GetComponent<Player>().currHP.Value + "|" + playerChar2.GetComponent<Player>().maxHP.Value + "]";
                coinTextP2.text = "Coins: [" + playerChar2.GetComponent<Player>().numCoins.Value + "]";
            }
            else //When the player is dead do something to end the game and such
            {
                print("Game over, saving scores");
                //Destroy(playerChar);
                //Player 1 and player 2 scores saved for persistence.
                int finalScore = playerChar.GetComponent<Player>().numCoins.Value;
                int finalScore2 = playerChar2.GetComponent<Player>().numCoins.Value;

                scoreSave.coinScore = finalScore;
                scoreSave.coinScore2 = finalScore2;

                if (finalScore > finalScore2)
                { //Player 1 wins by score
                    setWinScreen(redCol, blueCol, redCol, "Player 1 Wins");
                    print("Player 1 Wins");
                }
                if (finalScore < finalScore2)
                { //Player 2 wins by score
                    setWinScreen(redCol, blueCol, blueCol, "Player 2 Wins");
                    print("Player 2 Wins");
                }
                else if (finalScore == finalScore2) 
                { // both players tied
                    setWinScreen(redCol, blueCol, Color.magenta, "The game is a Tie");
                    print("Tied Game!");
                }
                
                //SceneManager.LoadScene("Game Over");
                if (IsServer)
                {
                    NetworkManager.SceneManager.LoadScene("Game Over", LoadSceneMode.Single);                   
                    //playerChar.GetComponent<NetworkObject>().Despawn(true);
                    //playerChar2.GetComponent<NetworkObject>().Despawn(true);
                    //theCoin.GetComponent<NetworkObject>().Despawn(true);
                }
            }
        }  
    }

    private void setWinScreen(Color plrColor, Color plr2Color, Color winnerColor, string plrWinner)
    {
        scoreSave.playerColor = plrColor;
        scoreSave.player2Color = plr2Color;
        scoreSave.winnerColor = winnerColor;
        scoreSave.winner = plrWinner;

    }

    private void FixedUpdate()
    {
        //if a coin is not found on the map, spawn a new one.
        //print(GameObject.Find("Coin") == null && GameObject.Find("Coin(Clone)") == null);
        if (!IsServer)
        {
            return;
        }
        //print(playersList + " " + numPlayers);
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

        theCoin = Instantiate(coin, spawnLoc, Quaternion.Euler(0, 0, 0));
        theCoin.GetComponent<NetworkObject>().Spawn();

    }

}
