using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombinable
{
    Color color { get; }
    bool CheckCombinable(Color? color);
}
