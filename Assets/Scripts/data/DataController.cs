using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;


public class DataController : MonoBehaviour //게임데이터 컨트롤위한 클래스
{


    public string GameDataFileName = "HAB_SaveData.json";
    public GameData _gameData;

    public static DataController instace;

    private void Awake()
    {
        instace = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadGameData();
    }

    public void LoadGameData() //게임 정보를 불러온다
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("Load complete");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("New Save Generated");
            _gameData = new GameData();
        }
    }
    public void SaveGameData() //게임정보를 저장한다
    {
        string ToJsonData = JsonUtility.ToJson(_gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);

        Debug.Log("Save Complete");
        Debug.Log("Saved : "+_gameData.scores);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
