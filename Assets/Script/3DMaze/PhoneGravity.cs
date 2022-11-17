using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneGravity : MonoBehaviour
{
    [SerializeField]Rigidbody rb;
    [SerializeField]float gravityMagnitude;
    bool useGyro;

    Vector3 gravityDir;

    void Start()
    {
        if(SystemInfo.supportsGyroscope)
        {
            Debug.Log("support");
            useGyro = true;
            Input.gyro.enabled = true;

        }
    }

    void Update()
    {
        var inputDir = useGyro ? Input.gyro.gravity : Input.acceleration;

        // Debug.Log(inputDir);

        //atur arah lagi, karena kamera berbeda orientasi dgn world game
        gravityDir = new Vector3
        (
            inputDir.x,
            inputDir.z,
            inputDir.y
        );

    }

    void FixedUpdate()
    {
        // Debug.Log("adding Force");
        //menggunakan constant acceleration krn gravity = acceleration
        rb.AddForce(gravityDir*gravityMagnitude,ForceMode.Acceleration);
    }
}
