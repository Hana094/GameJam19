using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Obstacle", menuName ="Obstacles")]
public class Obstacle : ScriptableObject
{

    public int[] size;// 0 is the x, 1 is the y

    public new string name;

    public GameObject body;
}
