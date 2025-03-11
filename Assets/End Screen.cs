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

    // Start is called before the first frame update
    void Start()
    {
        scoreText.color = Color.red;
        scoreText.text = "P1 Collected: " + scoreSave.coinScore;

        scoreTextP2.color = Color.blue;
        scoreTextP2.text = "P2 Collected: " + scoreSave.coinScore2;

        winnerText.color = scoreSave.playerColor;
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
    }
}
