using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SelectColorViewer : MonoBehaviour
{
    [SerializeField] private Image[] selectedColorImages;

    public void SetColor(Color color, int index)
    {
        selectedColorImages[index].color = color;
    }

    public void ResetColor()
    {
        for(int i = 0; i < selectedColorImages.Length; i++)
        {
            selectedColorImages[i].color = Color.white;
        }
    }
}
