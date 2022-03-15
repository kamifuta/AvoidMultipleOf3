using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour, ICombinable
{
    [SerializeField] private Text numText;
    [SerializeField] private AnimationCurve scaleCurve;

    private EffectGenerator effectGenerator;
    private ScoreManager scoreManager;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    public  Color color { get; private set; }
    public int num { get; private set; }
    public float radius { get; private set; }
    private float lastArea;
    private float currentArea;

    private void Start()
    {
        effectGenerator = FindObjectOfType<EffectGenerator>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        effectGenerator.PlayGenerateEF(transform.position);

        this.ObserveEveryValueChanged(x => x.color)
            .Subscribe(x =>
            {
                spriteRenderer.color = x;
            })
            .AddTo(this);

        this.ObserveEveryValueChanged(x => x.num)
            .Subscribe(x =>
            {
                numText.text = x.ToString();
                if (x % 3 == 0) //”Žš‚ª3‚Ì”{”‚É‚È‚Á‚½‚Æ‚«
                {
                    SEManager.Instance.Play(SEPath.BUBBLE,pitch:0.3f);
                    gameManager.MadeMultipleOfThree(num,transform.position);
                    Destroy(gameObject);
                }
                else //”Žš‚ªŽO‚Ì”{”‚Å‚È‚¢‚Æ‚«
                {
                    SEManager.Instance.Play(SEPath.BUBBLE);
                    SetCircleScale();
                    gameManager.SetHighNumber(num);
                }
            })
            .AddTo(this);

        this.ObserveEveryValueChanged(x => x.num)
            .Skip(1)
            .Subscribe(x =>
            {
                if (x % 3 != 0) 
                {
                    scoreManager.AddScore(num);
                    effectGenerator.PlayCombineEF(transform.position);
                }
            })
            .AddTo(this);

        this.ObserveEveryValueChanged(x => x.radius)
            .Subscribe(x =>
            {
                lastArea = currentArea;
                currentArea = ValueDecider.GetArea(x);
                gameManager.AddArea(color, (currentArea - lastArea));
            })
            .AddTo(this);
    }

    public void SetCircleInfo(Color color, int num)
    {
        this.color = color;
        this.num = num;
    }

    private void SetCircleScale()
    {
        radius = ValueDecider.DecideRadius(num);
        var token = this.GetCancellationTokenOnDestroy();
        ExpandCircle(token).Forget();
    }

    private async UniTask ExpandCircle(CancellationToken token=default)
    {
        float time = 0;
        if (transform.localScale.x != 0)
        {
            time = (transform.localScale.x / 2f) / radius;
        }

        while (time < 1)
        {
            time += Time.deltaTime;
            var radius=this.radius*scaleCurve.Evaluate(time);
            transform.localScale = new Vector3(radius*2, radius*2, 1);
            await UniTask.DelayFrame(1, cancellationToken: token);
        }
    }

    public void Combine(int num)
    {
        this.num += num;
    }

    public bool CheckCombinable(Color? color)
    {
        if (color == null) return false;
        if (this.color == color)
        {
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        gameManager.DecreaseArea(color, currentArea);
    }
}
