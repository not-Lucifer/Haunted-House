using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StealthGame
{
    public class PlayerMovement : MonoBehaviour
    {
        public InputAction MoveAction;

        public float walkSpeed = 1.0f;
        public float turnSpeed = 20f;

        Animator m_Animator;
        Rigidbody m_Rigidbody;
        AudioSource m_AudioSource;
        Vector3 m_Movement;
        Quaternion m_Rotation = Quaternion.identity;
    
        // DEMO
        private List<string> m_OwnedKeys = new();

        void Start ()
        {
            m_Animator = GetComponent<Animator> ();
            m_Rigidbody = GetComponent<Rigidbody> ();
            m_AudioSource = GetComponent<AudioSource> ();
        
            MoveAction.Enable();
        }

        void FixedUpdate ()
        {
            var pos = MoveAction.ReadValue<Vector2>();
        
            float horizontal = pos.x;
            float vertical = pos.y;
        
            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize ();

            bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;
            m_Animator.SetBool ("IsWalking", isWalking);
        
            if (isWalking)
            {
                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.Play();
                }
            }
            else
            {
                m_AudioSource.Stop ();
            }

            Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation (desiredForward);
        
            m_Rigidbody.MoveRotation (m_Rotation);
            m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);
        }

        public void AddKey(string keyName)
        {
            m_OwnedKeys.Add(keyName);
        }

        public bool OwnKey(string keyName)
        {
            return m_OwnedKeys.Contains(keyName);
        }
    }
}