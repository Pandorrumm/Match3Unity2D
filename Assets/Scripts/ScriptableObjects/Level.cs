using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "World", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header ("Board Dimensions")] //размеры доски
    public int width;
    public int height;

    [Header ("Starting Tiles")] //Стартовые плитки
    public TileType[] boardLayout; //раскладка доски

    [Header("Available Circle")] //доступные круги, кидаем их из префаба 
    public GameObject[] circle;

    [Header ("Score Goals")]
    public int[] scoreGoals;
	
}
