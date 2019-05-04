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
	
	void Awake ()
    {
		if(gameData == null)
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

    private void Start()
    {
       
    }

    public void Save()
    {
        //создаём binary formatter, который может читать бинарные файлы
        BinaryFormatter formatter = new BinaryFormatter();

        //создаём поток (маршрут) из программы в файл
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);

        //создаём пустые  данные сохранения
        SaveData data = new SaveData();
        data = saveData;

        //сохраняем файлы
        formatter.Serialize(file, data);

        // закрываем поток
        file.Close();

        Debug.Log("СОХРАНИЛИ");
    }

    public void Load()
    {
        // проверяем, существует ли файл сохранения игры
        if (File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            //создаём binary formatter
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
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
        }
    }

    private void OnApplicationQuit() //При выходе из приложения
    {
        Save();
    }


    private void OnDisable()
    {
        Save();
    }

    void Update ()
    {
		
	}
}
