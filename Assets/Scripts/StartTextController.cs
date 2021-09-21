using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartTextController : MonoBehaviour
{
    [SerializeField] private AnimationCurve blinkCurve;
    [SerializeField] private Text text;

    private void Start()
    {
        StartCoroutine(BlinkText());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private IEnumerator BlinkText()
    {
        float time = 0;
        while (true)
        {
            while (time <= 1)
            {
                var value=blinkCurve.Evaluate(time);
                text.color = new Color(0, 0, 0, value);
                yield return null;
                time += Time.deltaTime;
            }

            while (time >= 0)
            {
                var value = blinkCurve.Evaluate(time);
                text.color = new Color(0, 0, 0, value);
                yield return null;
                time -= Time.deltaTime;
            }
        }
    }
}
