using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;
    [SerializeField] private InputAction MoveAction;

    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float turnSpeed = 20f;

    Rigidbody m_Rigidbody ;
    Vector3 m_Movement;
    Quaternion m_Rotation  = Quaternion.identity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_Animator = GetComponent<Animator> ();
        MoveAction.Enable();
    }

    void FixedUpdate ()
    {
        var pos = MoveAction.ReadValue<Vector2>();
        
        float horizontal = pos.x;
        float vertical = pos.y;
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
        
        m_Rigidbody.MoveRotation (m_Rotation);
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);

        bool hasHorizontalInput = !Mathf.Approximately(horizontal,0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical,0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking",isWalking);  
    }
}
