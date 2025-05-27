using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public Dictionary<string, bool> RoomTasks = new Dictionary<string, bool>();
    public Dictionary<string, bool> SpecialTasks = new Dictionary<string, bool>();
    private bool escapeTaskCompleted = false;
    private bool deathTaskCompleted = false;
    
    private GhostAgentController _ai;

    void Start()
    {
        RoomTasks.Add("Bedroom", false);
        RoomTasks.Add("Pills", false);
        RoomTasks.Add("Beautiful", false);
        RoomTasks.Add("Bath", false);
        RoomTasks.Add("Drink", false);
        RoomTasks.Add("Kitchen", false);
        RoomTasks.Add("Stupid", false);
        RoomTasks.Add("Escape", false);

        SpecialTasks.Add("Death", false);
    }

    void Update()
    {
        if (logging == null) logging = StartCoroutine(Logging());
    }

    public void CompleteTask(string roomName)
    {
        if (RoomTasks.ContainsKey(roomName))
        {
            RoomTasks[roomName] = true;
            Debug.Log($"{roomName} task completed!");
        }
        else if (SpecialTasks.ContainsKey(roomName))
        {
            SpecialTasks[roomName] = true;
            Debug.Log($"{roomName} task completed!");
        }

        if (roomName == "Escape")
        {
            escapeTaskCompleted = true;
        }
        else if (roomName == "Death")
        {
            deathTaskCompleted = true;
        }

        if (AreAllTasksCompleted())
        {
            var checkAllTasks = FindObjectOfType<CheckAllTasks>();
            var ai = FindObjectOfType<GhostAgentController>();
            StartCoroutine(checkAllTasks.ShowMessageAndPlayMusic());


            ai.StartChase();
        }

        if (FinalGameStateAchieved())
        {
            var gameManager = FindObjectOfType<GameManager>();
            gameManager.Save();
        }
    }

    public bool AreAllTasksCompleted()
    {
        if (escapeTaskCompleted || deathTaskCompleted)
        {
            return false;
        }

        if (SpecialTasks.ContainsKey("Death") && SpecialTasks["Death"])
        {
            return true;
        }

        foreach (var task in RoomTasks.Keys)
        {
            if (!RoomTasks[task] && !task.Equals("Escape")) return false;
        }
        return true;
    }

    private int countNoTasksCompleted()
    {
        return CompletedTasks().Count;
    }

    public List<string> CompletedTasks()
    {
        var tasks = new List<string>();
        foreach (var t in RoomTasks.Keys)
        {
            if (RoomTasks[t]) tasks.Add(t);
        }
        foreach (var t in SpecialTasks.Keys)
        {
            if (SpecialTasks[t]) tasks.Add(t);
        }
        return tasks;
    }

    public bool FinalGameStateAchieved()
    {
        return RoomTasks["Escape"] || SpecialTasks["Death"];
    }

    private Coroutine logging;
    IEnumerator Logging()
    {
        Debug.Log("<color=#00ffffff>TaskManager log:</color>");
        Debug.Log("Tasks Completed: <color=#dd00ddff>" + countNoTasksCompleted() + " </color>");
        Func<Boolean, string> color = b => b ? "#00ff00ff" : "#ff0000ff";
        foreach (var task in RoomTasks.Keys)
        {
            Debug.Log(new Func<bool, string>((b) => $"{task}: <color={color(b)}>{b}</color>")(RoomTasks[task]));
        }

        yield return new WaitForSeconds(2f);

        logging = null;
    }
}