using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum GameState
{
    Playing,
    Result,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private CircleGenerator circleGenerator;
    [SerializeField] private SelectColorViewer selectedColorViewer;
    [SerializeField] private UIController UIController;

    public GameState gameState { get; private set; }

    private Color[] useColor= { Color.red, Color.green, Color.blue };

    private int combineCount = 0;
    private int maxGenerateNum;
    private int highNumber;
    private float totalColorArea = 0;
    private float[] colorArea = { 0, 0, 0 };
    
    private const int FirstGenerateQuantity = 50;
    private const int GenerateQuantity = 10;
    private const int ExplosionGenerateQuantity=5;
    private const int GenerateSpan = 5;
    private const float totalArea = 90;

    // Start is called before the first frame update
    void Start()
    {
        //useColor = new Color[]{ Color.red, Color.green, Color.blue };
        UIController.SetSlider(useColor);
        gameState = GameState.Playing;
        maxGenerateNum = 50;

        for(int i = 0; i < FirstGenerateQuantity; i++)
        {
            var generatePos = GetGeneratePos(-4.5f, 4.5f, -3.5f, 0f, Vector3.zero);
            int num = GetGenerateCircleNum(1, maxGenerateNum);
            var color = GetGenerateCircleColor();
            circleGenerator.GenerateCircle(generatePos, color, num);
        }

        this.ObserveEveryValueChanged(x => x.combineCount)
            .Where(x => x >= GenerateSpan)
            .Subscribe(_ =>
            {
                combineCount = 0;
                selectedColorViewer.ResetColor();
                for (int i = 0; i < GenerateQuantity; i++)
                {
                    var generatePos = GetGeneratePos(-4.5f, 4.5f, -3.5f, 0f,Vector3.zero);
                    int num = GetGenerateCircleNum(1, maxGenerateNum);
                    var color = GetGenerateCircleColor();
                    circleGenerator.GenerateCircle(generatePos, color, num);
                }
            })
            .AddTo(this);

        Observable
            .CombineLatest(this.ObserveEveryValueChanged(x => x.colorArea[0]), this.ObserveEveryValueChanged(x => x.colorArea[1]), this.ObserveEveryValueChanged(x => x.colorArea[2]))
            .Subscribe(area =>
            {
                totalColorArea = area[0] + area[1] + area[2];
                UIController.UpdataColorSlider(totalArea, colorArea);
            })
            .AddTo(this);

        this.ObserveEveryValueChanged(x => x.totalColorArea)
            .Where(x => x / totalArea >= 1)
            .Subscribe(_ =>
            {
                gameState = GameState.Result;
                UIController.ShowGameOverPanel();
            })
            .AddTo(this);
    }

    private Vector3 GetGeneratePos(float minX, float maxX, float minY, float maxY, Vector3 offset)
    {
        var generatePos= new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0) + offset;

        if (generatePos.x > 3.5f)
        {
            generatePos.x = 3f;
        }
        else if(generatePos.x < -3.5f)
        {
            generatePos.x = -3f;
        }

        if (generatePos.y > 4.5f)
        {
            generatePos.y = 4f;
        }
        else if (generatePos.y < -4.5)
        {
            generatePos.y = -4f;
        }
        return generatePos;
    }

    private int GetGenerateCircleNum(int min, int max)
    {
        return ValueDecider.GetNotMultipleOfThree(min, max);
    }

    private Color GetGenerateCircleColor()
    {
        var random = Random.Range(0, 3);
        return useColor[random];
    }

    /// <summary>
    /// çáê¨Ç≥ÇÍÇΩÇ∆Ç´Ç…åƒÇŒÇÍÇÈ
    /// </summary>
    /// <param name="color"></param>
    public void CombinedCallback(Color color)
    {
        selectedColorViewer.SetColor(color, combineCount);
        combineCount++;
    }

    public void SetHighNumber(int num)
    {
        if (num > highNumber)
        {
            highNumber = num;
            maxGenerateNum = highNumber / 2 + 50;
        }
    }

    public void AddArea(Color color, float area)
    {
        if (area == 0) return;
        if (color == useColor[0])
        {
            colorArea[0] += area;
        }
        else if(color == useColor[1])
        {
            colorArea[1] += area;
        }
        else
        {
            colorArea[2] += area;
        }
    }

    public void DecreaseArea(Color color, float area)
    {
        if (area == 0) return;
        if (color == useColor[0])
        {
            colorArea[0] -= area;
        }
        else if (color == useColor[1])
        {
            colorArea[1] -= area;
        }
        else
        {
            colorArea[2] -= area;
        }
    }

    /// <summary>
    /// 3ÇÃî{êîÇ™çÏÇÁÇÍÇΩÇ∆Ç´Ç…åƒÇŒÇÍÇÈ
    /// </summary>
    public void MadeMultipleOfThree(int circleNum, Vector3 offset)
    {
        for (int i = 0; i < ExplosionGenerateQuantity; i++)
        {
            var generatePos = GetGeneratePos(-1.5f, 1.5f, -1.5f, 1.5f, offset);
            int num = GetGenerateCircleNum(circleNum/5, circleNum/2);
            var color = GetGenerateCircleColor();
            circleGenerator.GenerateCircle(generatePos, color, num);
        }
    }
}
