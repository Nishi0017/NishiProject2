using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLimitScript2 : MonoBehaviour
{
    private Vector3 pos;

    [SerializeField] SensorScript sensorScript;
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
        pos = transform.localPosition;

        sensorRadius = sensorScript.searchRadius;
        sensorAngle = sensorScript.searchAngle;

        xAcuteAngle = xAngleLimit < 90f;
        yAcuteAngle = yAngleLimit < 90f;
        zAcuteAngle = zAngleLimit < 90f;

        switch (moveDirection)
        {
            case MoveDirection.YZ:
                x_max = pos.x;
                y_angle = Mathf.Tan(yAngleLimit);
                break;
            case MoveDirection.XZ:
                y_max = pos.y;
                break;
            case MoveDirection.All:
                break;
        }
    }

    private void Update()
    {
        switch (moveDirection)
        {
            case MoveDirection.YZ:
                pos.z = Mathf.Clamp(transform.localPosition.z, -sensorRadius, sensorRadius);
                /*
                y_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.z * pos.z);
                pos.y = Mathf.Clamp(pos.z, -z_max, z_max);
                line = pos.z * y_angle;
                if(yAcuteAngle)
                {
                    pos.y = Mathf.Clamp(pos.y, -line, line);
                }
                else if(pos.z < 0)
                {
                    if(pos.y >= 0)
                    {
                        pos.y = System.Math.Max(pos.y, line);
                    }
                    else
                    {
                        pos.y = System.Math.Min(pos.y, -line);
                    }
                }
                */
                break;

            case MoveDirection.XZ:
                pos.x = Mathf.Clamp(transform.localPosition.x, -x_max, x_max);
                pos.z = Mathf.Clamp(transform.localPosition.z, -z_max, z_max);
                line = Mathf.Tan(xAngleLimit * Mathf.Deg2Rad) * pos.x;
                pos.z = Mathf.Clamp(pos.z, line, -line);
                line = Mathf.Tan(zAngleLimit * Mathf.Deg2Rad) * pos.z;
                pos.x = Mathf.Clamp(pos.x, line, -line);
                break;
            case MoveDirection.All:
                pos.x = Mathf.Clamp(transform.localPosition.x, -x_max, x_max);
                pos.y = Mathf.Clamp(transform.localPosition.y, -y_max, y_max);
                pos.z = Mathf.Clamp(transform.localPosition.z, -z_max, z_max);
                line = Mathf.Tan(xAngleLimit * Mathf.Deg2Rad) * pos.x;
                pos.y = Mathf.Clamp(pos.y, line, -line);
                line = Mathf.Tan(yAngleLimit * Mathf.Deg2Rad) * pos.y;
                pos.z = Mathf.Clamp(pos.z, line, -line);
                line = Mathf.Tan(zAngleLimit * Mathf.Deg2Rad) * pos.z;
                pos.x = Mathf.Clamp(pos.x, line, -line);
                break;

        }

        transform.localPosition = pos;
    }
}