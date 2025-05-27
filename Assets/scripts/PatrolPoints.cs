using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolPoints : MonoBehaviour
{
    public GameObject root;
    
    private List<List<Transform>> patrolPoints = new List<List<Transform>>();
    
    private Transform curr_point;
    private Transform prev_point;

    private void Awake()
    {
        List<List<Transform>> temp = new List<List<Transform>>();
        
        for(int i = 0; i < root.transform.childCount; i++)
        {
            temp.Add(new List<Transform>());
            for (int j = 0; j < root.transform.GetChild(i).transform.childCount; j++)
            {
                temp[i].Add(root.transform.GetChild(i).transform.GetChild(j));
            }
        }
        
        patrolPoints = temp;
    }


    public void OnDrawGizmos()
    {
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            if (i == 0)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            foreach (Transform p in patrolPoints[i])
            {
                Gizmos.DrawSphere(p.position, 0.5f); // Draw a sphere at each patrol point
            }
        }
        
        Gizmos.color = Color.white;
        if (curr_point != null)
                Gizmos.DrawSphere(curr_point.position, 0.5f);
    }

    public bool hasPoint()
    {
        return curr_point != null;
    }

    public Transform next()
    {
        Debug.Log("Get next point.");
        int index = Random.Range(0, root.transform.childCount);
        curr_point = root.transform.GetChild(index);
        return curr_point;
    }

    public Transform next_point()
    {
        int index = Random.Range(0, patrolPoints.Count);
        List<Transform> room = patrolPoints[index];
        index = Random.Range(0, room.Count);
        curr_point = room[index];
        return curr_point;

    }

    public void clearCurrentPoint()
    {
        curr_point = null;
    }

    public bool reachedCurrentPoint(Vector3 pos)
    {
        if (curr_point == null) return false;
        return Vector3.Distance(pos, curr_point.position) <= 0.2f;
    }
}
