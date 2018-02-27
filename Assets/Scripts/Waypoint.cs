using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] List<Waypoint> m_possibleWayPoints = null;
    void OnTriggerEnter(Collider collider)
    {
        AI ai = collider.gameObject.GetComponent<AI>();

        if (ai)
        {
            int num = (int)Random.Range(0.0f, m_possibleWayPoints.Count - 1);
            Waypoint nextWayPoint = m_possibleWayPoints[num];

            ai.SetWaypoint(nextWayPoint);
        }
    }
}
