using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLimitScript : MonoBehaviour
{
    [SerializeField] private Vector3 pos_min;
    [SerializeField] private Vector3 pos_max;
    private Vector3 pos;

    private void Update()
    {
        pos.x = Mathf.Clamp(this.transform.localPosition.x, pos_min.x, pos_max.x);
        pos.y = Mathf.Clamp(this.transform.localPosition.y, pos_min.y, pos_max.y);
        pos.z = Mathf.Clamp(this.transform.localPosition.z, pos_min.z, pos_max.z);
        transform.localPosition = pos;
    }
}
