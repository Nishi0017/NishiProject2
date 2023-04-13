using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ControlLimitScript : MonoBehaviour
{
    private Vector3 pos_min;
    private Vector3 pos_max;
    private Vector3 pos;

    [SerializeField] SensorScript sensorScript;
    private float sensorRadius;

    private float z_min;
    private float z_max;
    

    private void Start()
    {
        pos.x = 0f;

        float sensorAngle = sensorScript.searchAngle;
        sensorRadius = sensorScript.searchRadius;

        pos_min.y = -sensorRadius;
        pos_max.y = 0;
        z_min = -sensorRadius * Mathf.Sin(sensorAngle);
        z_max = sensorRadius * Mathf.Sin(sensorAngle);
    }

    private void Update()
    {

        pos.y = Mathf.Clamp(this.transform.localPosition.y, pos_min.y, pos_max.y);

        pos_min.z = Mathf.Sqrt(sensorRadius * sensorRadius - pos.y * pos.y);
        pos_max.z = -pos_min.z;

        pos.z = Mathf.Clamp(this.transform.localPosition.z, pos_min.z, pos_max.z);

        //pos.y = Mathf.Clamp(this.transform.localPosition.y, pos_min.y, pos_max.y);
        pos.z = Mathf.Clamp(this.transform.localPosition.z, z_min, z_max);

        transform.localPosition = pos;


    }
}
