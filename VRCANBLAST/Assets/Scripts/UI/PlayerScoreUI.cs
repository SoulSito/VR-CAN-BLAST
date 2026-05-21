using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        PlayerScore playerScore = GameMode.Instance.GetPlayerScore();

        scoreText.text = playerScore.ToString();
    }
}
