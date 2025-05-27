using UnityEngine;

public class EscapeTask : MonoBehaviour
{
    public string roomName = "Escape";
    private bool taskCompleted = false;
    private bool playerInTrigger = false;
    
    void Update()
    {
        if (playerInTrigger && !taskCompleted)
        {
            Debug.Log("<color=#0000ffff>The Player has Escaped</color>");
            taskCompleted = true;

            TaskManager taskManager = FindObjectOfType<TaskManager>();
            if (taskManager != null)
            {
                taskManager.CompleteTask(roomName);
            }
            else
            {
                Debug.LogError("TaskManager not found in the scene.");
            }
        }
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}
