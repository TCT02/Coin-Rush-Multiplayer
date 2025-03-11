using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] public ScoreSave scoreSave;
    [SerializeField] public TextMeshPro scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Coins Collected: " + scoreSave.coinScore;
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
