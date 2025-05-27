using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class EndStatsUISystem : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI tasksText;
    
    private EndStats _endStats;

    public void Start()
    {
        string saveContent = File.ReadAllText(Application.persistentDataPath + "/save.json");
        Debug.Log(saveContent);
         _endStats = JsonUtility.FromJson<EndStats>(saveContent);
         
         timerText.text += _endStats.Time.ToString();

         foreach (var t in _endStats.Tasks)
         {
             tasksText.text += "\n  " + t;
         }
         
             
             
    }
}

[System.Serializable]
public struct EndStats
{
    public float Time;
    public string[] Tasks;
}
