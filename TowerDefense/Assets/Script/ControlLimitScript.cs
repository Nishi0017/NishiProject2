using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLimitScript : MonoBehaviour
{
    private Vector3 pos;

    [SerializeField] GameObject sensorObject;
    private SensorScript sensorScript;
    private float sensorRadius;
    private float sensorAngle;

    public enum MoveDirection
    {
        XY,
        YZ,
        ZX,
        All
    }
    public MoveDirection moveDirection;

    private float x_max;
    private float y_max;
    private float z_max;

    private void Start()
    {
        sensorScript = sensorObject.GetComponent<SensorScript>();

        pos = transform.localPosition;

        sensorRadius = sensorScript.searchRadius;
        sensorAngle = sensorScript.searchAngle;


        switch (moveDirection)
        {
            case MoveDirection.XY:
                z_max = pos.y;
                break;
            case MoveDirection.YZ:
                x_max = pos.x;
                break;
            case MoveDirection.ZX:
                y_max = pos.z;
                break;

            case MoveDirection.All:
                break;
        }
    }

    private void Update()
    {
        switch (moveDirection)
        {            
            case MoveDirection.XY:
                pos.y = Mathf.Clamp(transform.localPosition.y, -sensorRadius, sensorRadius);
                x_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.y * pos.y);
                pos.x = Mathf.Clamp(transform.localPosition.x, -x_max, x_max);
                break;

            case MoveDirection.YZ:
                pos.z = Mathf.Clamp(transform.localPosition.z, -sensorRadius, sensorRadius);
                y_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.z * pos.z);
                pos.y = Mathf.Clamp(transform.localPosition.y, -y_max, y_max);
                break;

            case MoveDirection.ZX:
                pos.x = Mathf.Clamp(transform.localPosition.x, -sensorRadius, sensorRadius);
                z_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.x * pos.x);
                pos.z = Mathf.Clamp(transform.localPosition.z, -z_max, z_max);
                break;

            case MoveDirection.All:
                pos.z = Mathf.Clamp(transform.localPosition.z, -sensorRadius, sensorRadius);

                x_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.z * pos.z);
                pos.x = Mathf.Clamp(transform.localPosition.y, -x_max, x_max);

                y_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.z * pos.z);
                pos.y = Mathf.Clamp(transform.localPosition.x, -y_max, y_max);
                break;

        }

        transform.localPosition = pos;
    }
}