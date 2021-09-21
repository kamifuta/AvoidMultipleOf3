using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    [SerializeField] ParticleSystem combineEF;
    [SerializeField] ParticleSystem generateEF;

    public void PlayGenerateEF(Vector3 generatePos)
    {
        Instantiate(generateEF, generatePos, Quaternion.identity);
    }

    public void PlayCombineEF(Vector3 generatePos)
    {
        Instantiate(combineEF, generatePos, Quaternion.identity);
    }
}
