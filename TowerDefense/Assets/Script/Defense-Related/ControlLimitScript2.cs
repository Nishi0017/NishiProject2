using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLimitScript2 : MonoBehaviour
{
    private Vector3 pos;

    [SerializeField] GameObject sensorObject;

    private Vector3 centerPos;
    private SensorScript sensorScript;
    private float sensorRadius;
    private float sensorAngle;
    private float x_angle;
    private float y_angle;
    private float z_angle;
    private float line;

    public enum MoveDirection
    {
        XZ,
        YZ,
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
        sensorScript = sensorObject.GetComponent<SensorScript>();
        centerPos = sensorObject.transform.position - transform.position;

        pos = transform.localPosition;

        sensorRadius = sensorScript.searchRadius;
        sensorAngle = sensorScript.searchAngle;

        xAcuteAngle = xAngleLimit < 90f;
        yAcuteAngle = yAngleLimit < 90f;
        zAcuteAngle = zAngleLimit < 90f;

        switch (moveDirection)
        {
            case MoveDirection.XZ:
                y_max = pos.y;
                break;
            case MoveDirection.YZ:
                x_max = pos.x;
                y_angle = Mathf.Tan(yAngleLimit);
                break;

            case MoveDirection.All:
                break;
        }
    }

    private void Update()
    {
        switch (moveDirection)
        {
            case MoveDirection.XZ:
                pos.z = Mathf.Clamp(transform.localPosition.z, -sensorRadius, sensorRadius);
                x_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.z * pos.z);
                pos.x = Mathf.Clamp(transform.localPosition.x, -x_max, x_max);
                break;

            case MoveDirection.YZ:
                pos.z = Mathf.Clamp(transform.localPosition.z, -sensorRadius, sensorRadius);
                y_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.z * pos.z);
                pos.y = Mathf.Clamp(transform.localPosition.y, -y_max, y_max);
                break;


            case MoveDirection.All:
                pos.z = Mathf.Clamp(transform.localPosition.z, -sensorRadius, sensorRadius);

                x_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.z * pos.z);
                pos.x = Mathf.Clamp(transform.localPosition.x, -x_max, x_max);

                y_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.z * pos.z);
                pos.y = Mathf.Clamp(transform.localPosition.y, -y_max, y_max);
                break;

        }

        transform.localPosition = pos;
    }
}