using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemInteraction : MonoBehaviour
{
    public string roomName = "RoomName";
    public AudioSource externalAudioSource;

    private void OnEnable()
    {
        // Ensure the collider is set as a trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void OnDisable()
    {
        GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(OnSelectEntered);
    }

    private void Update()
    {
        // non-vr
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    //Debug.Log("player clicked on the item.");
                    CompleteTask();
                }
                else
                {
                    //Debug.Log("Raycast hit a different object: " + hit.transform.name);
                }
            }
            else
            {
                //Debug.Log("Raycast did not hit any object.");
            }
        }

        // close enough + camera looking correctly
        Ray lookRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit lookHit;
        if (Physics.Raycast(lookRay, out lookHit))
        {
            if (lookHit.transform == transform)
            {
                float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
                //Debug.Log("Distance to item: " + distance);
                if (distance <= 1.0f)
                {
                    //Debug.Log("player is looking and close enough");
                    CompleteTask();
                }
                else
                {
                    //Debug.Log("player is too far from item");
                }
            }
            else
            {
                //Debug.Log("look raycast hit a sumelse: " + lookHit.transform.name);
            }
        }
        else
        {
            //Debug.Log("Look raycast hit nothing :(");
        }
    }

    //VR
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        //Debug.Log("player interacted with item");
        CompleteTask();
    }

    // Trigger method for VR hand interaction
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player's hand
        if (other.CompareTag("PlayerHand"))
        {
            CompleteTask();
        }
    }

    public void CompleteTask()
    {
        TaskManager taskManager = FindObjectOfType<TaskManager>();
        if (taskManager != null)
        {
            taskManager.CompleteTask(roomName);
        }
        else
        {
            Debug.LogError("TaskManager not found");
        }

        if (externalAudioSource != null)
        {
            externalAudioSource.Play();
        }
        else
        {
            Debug.LogError("External AudioSource component missing.");
        }

        Destroy(gameObject);
    }
}