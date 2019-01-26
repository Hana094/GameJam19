using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Obstacle", menuName ="Obstacles")]
public class Obstacle : ScriptableObject
{

    public float[] size;// 0 is the x, 1 is the y, 3 is the high

    public new string name;

    public GameObject body;
}
/*
[CreateAssetMenu(fileName = "New Refugee", menuName = "Refugees")]
public class Refugee : ScriptableObject
{

    public int[] nedeedResources;// 0 Comestibles , 1 leña, 2 constrrucción

    public new string name;

    public GameObject body;
}*/
