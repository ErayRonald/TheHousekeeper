using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using File = System.IO.File;


public class GameManager : MonoBehaviour
{

    private float _timer;
    private TaskManager _taskManager;
    private SceneTransitionManager _sceneTransitionManager;
    
    private GameState _gameState;
    private bool _saved;
    
    public GameState GameState { get => _gameState; set => _gameState = value; }

    private void Awake()
    {
        Load();
    }

    // Start is called before the first frame update
    void Start()
    {
        _timer = Time.time;
        _taskManager = FindObjectOfType<TaskManager>();
        _sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
    }

    public void Save()
    {
        if (!_saved)
        {
            Debug.Log("<color=#00ffffff>Saving game state</color>");
            SaveGameState();
            _sceneTransitionManager.GoToSceneAsync(2);
            
            string fName = Application.persistentDataPath + "/save" + ".json";
            File.WriteAllText(fName, JsonUtility.ToJson(_gameState, true));
            _saved = true;
        }
    }

    private void SaveGameState()
    {
        _gameState.Time = Time.time - _timer;
        _gameState.Tasks = _taskManager.CompletedTasks();
    }
    
    private void Load()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
    }
}




[System.Serializable]
public struct GameState
{
    public float Time;
    public List<string> Tasks;
}
