using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class SaveData
{
    public bool[] isActiv;
    public int[] highScores;
    public int[] stars;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;
    private EndGameManager endGameManager;
    private Board board;

    void Start()
    {
        endGameManager = FindObjectOfType<EndGameManager>();
        board = FindObjectOfType<Board>();
    }

    void Awake ()
    {
        
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Load();
    }  

    public void Save()
    {
        //создаём binary formatter, который может читать бинарные файлы
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.fun";
        //создаём поток (маршрут) из программы в файл
        //FileStream file = File.Create(Application.persistentDataPath + "/player.fun");

        FileStream file = new FileStream(path, FileMode.Create);

        //создаём пустые  данные сохранения
        SaveData data = new SaveData();
        data = saveData;
        
        //if(endGameManager.tryAgainPanel )
        //{
        //    saveData.stars[board.level -1] = 0;
        //}
        //сохраняем файлы
        formatter.Serialize(file, data);

        // закрываем поток
        file.Close();

        Debug.Log("СОХРАНИЛИ");
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/player.fun";
        // проверяем, существует ли файл сохранения игры
        if (File.Exists(path))
        {
            //создаём binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream file = new FileStream(path, FileMode.Open);
            //FileStream file = File.Open(Application.persistentDataPath + "/player.fun", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Загрузили");
        }
        else 
        {
            saveData = new SaveData();
            saveData.isActiv = new bool[100];
            saveData.stars = new int[100];
            saveData.highScores = new int[100];
            saveData.isActiv[0] = true; // первый уровень самый
            Debug.Log("Открыт первый уровень");
        }
    }

    private void OnDisable()
    {
        Save();
    }

    public void OnApplicationQuit() //При выходе из приложения
    {
        Application.Quit();
        Save();
        Debug.Log("Вышли из игры");
    }
   

    void Update ()
    {
		
	}
}
