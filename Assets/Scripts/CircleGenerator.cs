using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Transform circleParent;

    public void GenerateCircle(Vector3 generatePos, Color color, int num)
    {
        var circleObj=Instantiate(circlePrefab, generatePos, Quaternion.identity);
        circleObj.transform.SetParent(circleParent);
        var circle = circleObj.GetComponent<Circle>();
        circle?.SetCircleInfo(color, num);

        //SEManager.Instance.Play(SEPath.BUBBLE,pitch:0.1f);
    }
}
