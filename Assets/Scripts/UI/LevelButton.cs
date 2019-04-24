using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelButton : MonoBehaviour
{
    [Header ("Active Stuff")] //Активные вещи
    public bool isActive;
    public Sprite activeSprite; // открытый
    public Sprite lockedSprite; // закрытый
    private Image buttonImage;
    private Button myButton;
    private int starsActive;

    [Header ("Level UI")]
    public Image[] stars; //заполненные звёзды, дочерние в юнити
    public Text levelText;
    public int level;
    public GameObject confirmPanel; //панель с баксами и звездами
    

    private GameData gameData;

	void Start ()
    {
        gameData = FindObjectOfType<GameData>();
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        LoadData();
        ActivateStars();
        ShowLevel();
        DecideSprite();

    }
	
    void LoadData()
    {
        //Есть ли игровые данные?
        if(gameData != null)
        {
            //принять решение, если уровень активен
            if(gameData.saveData.isActiv[level - 1])
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
            //решить, сколько звезд активировать
            starsActive = gameData.saveData.stars[level - 1];
        }
    }

    void ActivateStars()
    {       
        for (int i = 0; i < starsActive; i ++)
        {

            stars[i].enabled = true;
        }
    }

    void DecideSprite() // решение по спрайту - открытый или закрытый уровень
    {
        if(isActive)
        {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelText.enabled = true;
        }
        else
        {
            buttonImage.sprite = lockedSprite;
            myButton.enabled = false;
            levelText.enabled = false;
        }
    }

    void ShowLevel()
    {
        levelText.text = "" + level;
    }

	void Update ()
    {
		
	}

   public void ConfirmPanel(int level)
    {
        confirmPanel.GetComponent<ConfirmPanel>().level = level;
        confirmPanel.SetActive(true);
    }


}
