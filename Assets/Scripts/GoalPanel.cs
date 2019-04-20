using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPanel : MonoBehaviour
{
    public Image thisImage; // картинка цели GoalImage из GoalPrefab
    public Sprite thisSprite;
    public Text thisText;  // Text из GoalPrefab
    public string thisString;
	
	void Start ()
    {
        Setup();
	}
	
	void Setup() //настройка
    {
        thisImage.sprite = thisSprite;
        thisText.text = thisString;
    }
}
