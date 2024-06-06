using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSpin : MonoBehaviour
{
    public float SpinSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(SpinSpeed * Time.deltaTime, 0, 0);
    }
}
