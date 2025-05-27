using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    public InputActionProperty activateAction; // Add an InputActionProperty field
    private Animator animator;
    public TextMeshProUGUI debugText; // Reference to the TextMeshProUGUI component

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isActivated = activateAction.action.ReadValue<bool>();
        string actionName = activateAction.action.name;
        string controlPath = activateAction.action.activeControl?.path;
        animator.SetBool("isGrabbing", isActivated);
        debugText.text = $"Action: {actionName}, Control: {controlPath}, Value: {isActivated}"; 

    }
}