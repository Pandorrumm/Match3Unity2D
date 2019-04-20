using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "World", menuName = "World")] // в юнити create assets папкаб там можно выбрать world
public class World : ScriptableObject
{
    public Level[] levels;

}
