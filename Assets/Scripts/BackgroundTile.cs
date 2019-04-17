using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints; //сколько нужно урона блоку
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(hitPoints <=0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage) //урон
    {
        hitPoints -= damage;
        MakeLighter();
    }

    void MakeLighter() //Сделать легче
    {
        //взять текущий цвет
        Color color = sprite.color;

        // получить текущее альфа-значение цвета и разрезать его пополам
        float newAlpha = color.a * .5f;
        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
