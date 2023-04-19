using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class ControlLimitScript : MonoBehaviour
{
    private Vector3 pos;

    [SerializeField] SensorScript sensorScript;
    private float sensorRadius;
    private float sensorAngle;
    private float angle;
    private float line;

    private float y_max;
    private float z_max;




    private void Start()
    {
        pos.x = 0f;

        sensorRadius = sensorScript.searchRadius;
        sensorAngle = sensorScript.searchAngle;
        angle = Mathf.Tan(sensorAngle);
        

        y_max = 0;
    }

    private void Update()
    {
        pos.y = Mathf.Clamp(transform.localPosition.y, -sensorRadius, y_max);

        z_max = Mathf.Sqrt(sensorRadius * sensorRadius - pos.y * pos.y);

        pos.z = Mathf.Clamp(transform.localPosition.z, -z_max, z_max);

        line = pos.y * angle;
        pos.z = Mathf.Clamp(pos.z, line, -line);

        transform.localPosition = pos;

    }
}
