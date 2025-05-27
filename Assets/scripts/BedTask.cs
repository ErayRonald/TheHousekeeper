using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class BedTask : MonoBehaviour
{
    public string roomName = "Bedroom";
    private bool taskCompleted = false;
    private bool playerInTrigger = false;
    private float initialHeight;

    public TMP_Text Text;
    public Transform CameraOffset;
    public CharacterController Characterco;

    void Start()
    {
        // Get the initial height of the XR Origin camera
    }

    void Update()
    {
        

        if (playerInTrigger && !taskCompleted)
        {
            float currentHeight = Camera.main.transform.position.y;
            float heightDifference = currentHeight - initialHeight;

            // Text.SetText($"trigger: {playerInTrigger} \n" +
            //               $"height: {heightDifference} \n");
            
            

            // Check if the player has gone above the initial height
            if (heightDifference > 0.05f) // Adjust the threshold as needed
            {
                Debug.Log("<color=#0000ffff>Player has gone above the initial height.</color>");
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

                AudioSource audio = GetComponent<AudioSource>();
                if (audio != null)
                {
                    Debug.Log("Playing audio.");
                    audio.Play();
                }
                else
                {
                    Debug.LogError("AudioSource component missing on BedTask GameObject.");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            initialHeight = Camera.main.transform.position.y;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    }
}