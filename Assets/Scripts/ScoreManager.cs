using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private UIController UIController;

    private int highScore;
    private int score=0;

    private readonly string HighScoreKey = "HighScore";

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UIController.UpdataHighScoreText(highScore);

        this.ObserveEveryValueChanged(x => x.score)
            .Subscribe(_ =>
            {
                UIController.UpdataScoreText(score);
                if (score > highScore)
                {
                    SaveHighScore();
                    UIController.UpdataHighScoreText(highScore);
                }
            })
            .AddTo(this);
    }

    public void AddScore(int num)
    {
        score += num;
    }

    public void SaveHighScore()
    {
        highScore = score;
        PlayerPrefs.SetInt(HighScoreKey, highScore);
    }
}
