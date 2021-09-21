using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueDecider
{
    public static float DecideRadius(int num)
    {
        if (num < 50)
        {
            return 0.25f;
        }
        else if (num < 100)
        {
            return 0.4f;
        }
        else if (num < 300)
        {
            return 0.5f;
        }
        else if (num < 500)
        {
            return 0.75f;
        }
        else if (num < 1000)
        {
            return 1f;
        }
        return 1.25f;
    }

    public static float GetArea(float radius)
    {
        return radius * radius * Mathf.PI;
    }

    public static int GetNotMultipleOfThree(int min, int max)
    {
        var n = 0;
        while (n % 3 == 0)
        {
            n=Random.Range(min, max + 1);
        }
        return n;
    }
}
