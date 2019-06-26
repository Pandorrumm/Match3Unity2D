using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints; //сколько нужно урона блоку
    private SpriteRenderer sprite;
    private GoalManager goalManager;
    public Sprite hitSprite;
    // private Board board;
    // public GameObject slimeDestroy;
    //public GameObject lockDestroy;
    // private BrickDamage brickDamage;
    // private Animator anim;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        goalManager = FindObjectOfType<GoalManager>();
        sprite = GetComponent<SpriteRenderer>();
       // board = GameObject.FindWithTag("Board").GetComponent<Board>();
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
        Invoke("MakeLighter", 0.2f);
        //MakeLighter();
      
    }

    void MakeLighter() //Сделать легче
    {
        //взять текущий цвет
        // Color color = sprite.color;
        //// brickDamage.BreakBrick();
        // // получить текущее альфа-значение цвета и разрезать его пополам
        // float newAlpha = color.a * 0.4f;
        // sprite.color = new Color(color.r, color.g, color.b, newAlpha);
        Music.PlaySound("Concrete Damage");
        GetComponent<SpriteRenderer>().sprite = hitSprite;
    }

   
}
