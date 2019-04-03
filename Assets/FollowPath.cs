using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform goal;
    float speed = 5f;
    float accuracy = 1f;
    float rotSpeed = 2f;
    public GameObject wpManager;
    GameObject[] wps;
    GameObject currentNode;
    int currentWp = 0;
    Graph g;

    private void Start()
    {
        wps = wpManager.GetComponent<WPManager>().wayPoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[2];
    }

    public void GoToHeli()
    {
        g.AStar(currentNode, wps[0]);
        currentWp = 0;
    }

    public void GoToRuins()
    {
        g.AStar(currentNode, wps[9]);
        currentWp = 0;
    }

    private void LateUpdate()
    {
        if (g.getPathLength() == 0 || currentWp == g.getPathLength())
        {
            return;
        }

        currentNode = g.getPathPoint(currentWp);

        if (Vector3.Distance(
            g.getPathPoint(currentWp).transform.position,
            transform.position) < accuracy)
        {
            currentWp++;
        }

        if (currentWp < g.getPathLength())
        {
            goal = g.getPathPoint(currentWp).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x,
                transform.position.y,
                goal.position.z);
            Vector3 direction = lookAtGoal - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * rotSpeed);
            transform.Translate(0, 0, Time.deltaTime * speed);
        }
    }
}
