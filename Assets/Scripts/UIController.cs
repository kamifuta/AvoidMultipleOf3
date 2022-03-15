using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Slider[] areaSlider;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void SetSlider(Color[] useColor)
    {
        for (int i = 0; i < areaSlider.Length; i++)
        {
            areaSlider[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = useColor[i];
            areaSlider[i].value = 0;
        }
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void UpdataScoreText(int score)
    {
        scoreText.text = $"SCORE:{score}";
    }

    public void UpdataHighScoreText(int highScore)
    {
        highScoreText.text = $"HIGH\nSCORE:{highScore}";
    }

    public void UpdataColorSlider(float totalArea, float[] colorArea)
    {
        areaSlider[0].value = colorArea[0] / totalArea;
        areaSlider[1].value = (colorArea[0] + colorArea[1]) / totalArea;
        areaSlider[2].value = (colorArea[0] + colorArea[1] + colorArea[2]) / totalArea;
    }
}
