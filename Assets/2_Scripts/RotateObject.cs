using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.forward * Time.deltaTime  * 360.0f);
    }
}
