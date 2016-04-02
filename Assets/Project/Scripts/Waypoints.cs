using UnityEngine;
using System.Collections.Generic;

public class Waypoints : MonoBehaviour
{
    void OnDrawGizmos()
    {
        var gizmos = new List<GameObject>();
        foreach (Transform child in transform)
        {
            gizmos.Add(child.gameObject);
        }

        for (int i = 0; i < gizmos.Count; i++)
        {
            Vector3 start = gizmos[i].transform.position;

            int destIndex = i + 1;
            if (i == gizmos.Count - 1) destIndex = 0;
            Vector3 destination = gizmos[destIndex].transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start, destination);
        }
    }
}

