using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Player : MonoBehaviour {
    public Waypoints CurrentRoute;
    public float MoveSpeed = 10f;
    public float RotationSpeed = 1f;

    private bool inRoute;
    private List<GameObject> route;
    private CharacterController controller;
    private int? nextLocation;
    private Vector3 targetPosition;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();

        if (CurrentRoute == null)
        {
            throw new UnityException("Current Route must be set");
        }

        LoadRoute();
    }
	
	// Update is called once per frame
	void Update () {
        if (!inRoute)
        {
            FindNextLocation();
        }
        else
        {
            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance < 3)
            {
                inRoute = false;
            }
            else
            {
                var targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

                Vector3 forward = transform.TransformDirection(Vector3.forward);
                controller.SimpleMove(forward * MoveSpeed);
            }
        }
    }

    public void SpeedChanged(object obj)
    {
        double speed = (double)obj;
        MoveSpeed = Convert.ToSingle(speed) * 3;
    }

    private void LoadRoute()
    {
        // Speed
        // Distance
        // Heart Rate
        // Elapsed Time
        // Lap
        // Calories

        route = new List<GameObject>();
        foreach (Transform child in CurrentRoute.transform)
        {
            route.Add(child.transform.gameObject);
        }
    }

    private void FindNextLocation()
    {
        inRoute = true;

        if (nextLocation == null || nextLocation == route.Count - 1)
        {
            nextLocation = 0;
        }
        else
        {
            ++nextLocation;
        }

        targetPosition = route[nextLocation.Value].transform.position;
    }
}
