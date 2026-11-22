using UnityEngine;
using UnityEngine.InputSystem;

public class HandsInputs : MonoBehaviour
{
    [SerializeField] InputActionProperty gripAction;
    [SerializeField] InputActionProperty triggerAction;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float grip = gripAction.action.ReadValue<float>();
        float trigger = triggerAction.action.ReadValue<float>();
        
        animator.SetFloat("Grip", grip);
        animator.SetFloat("Trigger", trigger);
    }
}
