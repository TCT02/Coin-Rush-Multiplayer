using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] public ScoreSave scoreSave;
    [SerializeField] public TextMeshPro scoreText;
    [SerializeField] public TextMeshPro scoreTextP2;
    [SerializeField] public TextMeshPro winnerText;
    [SerializeField] Color redCol;
    [SerializeField] Color blueCol;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.color = scoreSave.playerColor;
        scoreText.text = "P1 Collected: " + scoreSave.coinScore;

        scoreTextP2.color = scoreSave.player2Color;
        scoreTextP2.text = "P2 Collected: " + scoreSave.coinScore2;

        winnerText.color = scoreSave.winnerColor;
        winnerText.text = scoreSave.winner;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    SceneManager.LoadScene("Main Menu");
                    break;
            }

        }
        if (Input.GetMouseButton(0)) //if tapping
        {
            print("Loading Game");
            SceneManager.LoadScene("Main Menu");
        }
    }
}
