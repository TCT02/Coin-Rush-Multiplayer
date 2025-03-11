using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSave", menuName = "Score")]

public class ScoreSave : ScriptableObject
{
    public string sceneName;
    public bool isNextScene = true;
    public int coinScore;
    [SerializeField] public ScoreSave scoreSave;

}
