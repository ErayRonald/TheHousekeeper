using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty activateAction;
    public Animator handAnimator;
    public TextMeshProUGUI vrText; // Add a TextMeshProUGUI field

    // Start is called before the first frame update
    void Start()
    {
        vrText.text = ""; // Initialize the text to be empty
    }

    // Update is called once per frame
    void Update()
    {
        bool isActivated = activateAction.action.ReadValue<bool>();
        Debug.Log(isActivated);

        handAnimator.SetBool("isGrabbing", isActivated);

        if (isActivated)
        {
            vrText.text = "Activated!"; // Set the text when activated
        }
        else
        {
            vrText.text = ""; // Clear the text when not activated
        }
    }
}