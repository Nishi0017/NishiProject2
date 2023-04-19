using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLimitScript3 : MonoBehaviour
{
    private Vector3 forward;
    private Transform body;
    private float distance;

    private float sensorRadius;
    private float sensorAngle;
    private float x_angle;
    private float y_angle;
    private float z_angle;
    private float line;

    public enum MoveDirection
    {
        YZ,
        XZ,
        All
    }
    public MoveDirection moveDirection;

    public float xAngleLimit = 45f;
    private bool xAcuteAngle;
    public float yAngleLimit = 45f;
    private bool yAcuteAngle;
    public float zAngleLimit = 45f;
    private bool zAcuteAngle;

    private float x_max;
    private float y_max;
    private float z_max;

    private void Start()
    {
        forward = transform.forward;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, body.position);
        if(distance > sensorRadius)
        {

        }
    }
}
