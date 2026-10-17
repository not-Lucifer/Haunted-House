using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointPatrol : MonoBehaviour
{

    public float movespeed = 1.0f;
    public Transform[] waypoints;
    private Rigidbody m_Rigidbody;
    int m_CurrentWaypointIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Transform currentwaypoint = waypoints[m_CurrentWaypointIndex];
        Vector3 currentToTarget = currentwaypoint.position - m_Rigidbody.position;

        if (currentToTarget.magnitude < 0.1f)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex+1)%waypoints.Length;
        }

        Quaternion forwardRotation = Quaternion.LookRotation(currentToTarget);
        m_Rigidbody.MoveRotation (forwardRotation); 
        m_Rigidbody.MovePosition(m_Rigidbody.position + currentToTarget.normalized*movespeed*Time.deltaTime);
    }
}
