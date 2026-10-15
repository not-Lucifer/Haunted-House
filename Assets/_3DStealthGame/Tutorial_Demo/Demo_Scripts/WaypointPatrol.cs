using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StealthGame
{
    public class WaypointPatrol : MonoBehaviour
    {
        public float moveSpeed = 1.0f;
        public Transform[] waypoints;

        private Rigidbody m_RigidBody;
        int m_CurrentWaypointIndex;

        void Start ()
        {
            m_RigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate ()
        {
            Transform currentWaypoint = waypoints[m_CurrentWaypointIndex];
            Vector3 currentToTarget = currentWaypoint.position - m_RigidBody.position;

            if (currentToTarget.magnitude < 0.1f)
            {
                //very close to the waypoint, get to the next waypoint
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            }
        
            //find the rotation to orient the rigidbody toward the waypoint
            //This will be a sharp change toward that direction, this could be made more gradual if wanted by only rotating
            //at a given speed.
            Quaternion forwardRotation = Quaternion.LookRotation(currentToTarget);
            m_RigidBody.MoveRotation(forwardRotation);
        
            //move toward the waypoint at the set speed
            //currentToTarget is normalized before multiplying by speed because we only want the direction and not the length
            m_RigidBody.MovePosition(m_RigidBody.position + currentToTarget.normalized * moveSpeed * Time.deltaTime);
        }
    }
}