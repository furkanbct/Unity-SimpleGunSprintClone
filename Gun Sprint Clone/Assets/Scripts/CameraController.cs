using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;
    float tempSpeed;
    Pistol pistol;
    // Start is called before the first frame update
    void Start()
    {
        pistol = target.GetComponent<Pistol>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (pistol.GetComponent<Rigidbody>().velocity.magnitude > 25)
        {
            if (tempSpeed != speed * 2)
            {
                tempSpeed = Mathf.Lerp(speed, speed * 2, Time.deltaTime);
            }
        }
        else
        {
            tempSpeed = speed;
        }
        Vector3 targetPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * tempSpeed);
        transform.position = smoothPos;
    }
}
