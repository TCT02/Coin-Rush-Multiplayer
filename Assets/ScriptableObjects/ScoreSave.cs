using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSave", menuName = "Score")]

public class ScoreSave : ScriptableObject
{
    public string sceneName;
    public bool isNextScene = true;
    public int coinScore;
    public int coinScore2;
    public int winnerScore;
    public Color playerColor;
    public Color player2Color;
    public Color winnerColor;
    public string winner;
    [SerializeField] public ScoreSave scoreSave;
    [SerializeField] public ScoreSave scoreSave2;

}
