using UnityEngine;
using System.Collections;
using System;

public class OtherPlayer : MonoBehaviour {

    private string id;
    private Vector3 targetRotation;
    private Vector3 targetPosition;
    public float RotationSpeed = 1f;
    public float PositionSpeed = 1f;

    public string Id { get { return id; } set { id = value; } }

    public void SlerpTo(Vector3 position, Vector3 rotation)
    {
        targetRotation = rotation;
        targetPosition = position;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetRotation - transform.position), RotationSpeed * Time.deltaTime);
        float step = PositionSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

}
