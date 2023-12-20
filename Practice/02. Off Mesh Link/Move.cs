using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    NavMeshAgent agent;

    void Start()
    {
        // Get the NavMeshAgent component attached to the GameObject
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to the ground from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Set the destination of the NavMeshAgent to the clicked position
                agent.SetDestination(hit.point);
                
            }
        }

        if(Input.GetKeyDown(KeyCode.F1))
        {
            agent.Warp(Vector3.zero);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            agent.Warp(Vector3.forward * 100f);
        }
    }
}
