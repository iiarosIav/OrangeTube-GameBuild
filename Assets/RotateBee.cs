using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBee : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, Time.deltaTime * 2000, 0);
    }
}
