using System.Collections.Generic;
using UnityEngine;

public class SampleDebug : MonoBehaviour
{
    private int num;

    private void Start()
    {
        num = 0;

        Debug.Log("First");
        num = 1;

        Debug.Log("Second");
        num = 2;
    }
}
