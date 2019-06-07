using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints; //сколько нужно урона блоку
    private SpriteRenderer sprite;
    private GoalManager goalManager;
   // private BrickDamage brickDamage;

    private void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        sprite = GetComponent<SpriteRenderer>();
        //brickDamage = FindObjectOfType<BrickDamage>();
    }

    private void Update()
    {
        if(hitPoints <= 0)
        {
            if(goalManager != null)
            {
                goalManager.CompareGoal(this.gameObject.tag);
                goalManager.UpdateGoals();
            }
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage) //урон
    {
        hitPoints -= damage;
        MakeLighter();
        //brickDamage.BreakBrick();
    }

    void MakeLighter() //Сделать легче
    {
        //взять текущий цвет
        Color color = sprite.color;
       // brickDamage.BreakBrick();
        // получить текущее альфа-значение цвета и разрезать его пополам
        float newAlpha = color.a * .5f;
        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
