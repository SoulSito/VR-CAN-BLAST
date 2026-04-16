using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;

    int seconds = 60;
    int score = 0;

    internal void StartTimer(int startingSeconds)
    {
        seconds = startingSeconds;

        StartCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        while(GameMode.Instance.gameState == GameState.Playing)
        {
            if(seconds <= 0)
            {
                GameMode.Instance.TimeEnded();
            }
            
            timerText.text = seconds.ToString();

            seconds--;

            yield return new WaitForSeconds(1f);
        }
    }

    internal void IncreaseScore(int increaseAmount)
    {
        score += increaseAmount;

        scoreText.text = score.ToString();
    }
}
