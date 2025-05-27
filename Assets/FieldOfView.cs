using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    
    public Transform head;

    void Start() {
        StartCoroutine ("FindTargetsWithDelay", .2f);
    }

    public void OnDrawGizmos()
    {
        foreach (var visibleTarget in visibleTargets)
        {
            Gizmos.DrawLine(visibleTarget.transform.position, head.transform.position);
        }
    }

    public bool ContainsPlayer( out Transform target )
    {
        target = null;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (visibleTargets.Contains(player.transform))
        {
            target = player.transform;
            return true;
        }

        return false;
    }

    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds (delay);
            FindVisibleTargets ();
        }
    }

    void FindVisibleTargets() {
        visibleTargets.Clear ();
        Collider[] targetsInViewRadius = Physics.OverlapSphere (head.transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius [i].transform;
            Vector3 dirToTarget = (target.position - head.transform.position).normalized;
            if (Vector3.Angle (head.transform.up, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance (head.transform.position, target.position);

                if (!Physics.Raycast (head.transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    visibleTargets.Add (target);
                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
