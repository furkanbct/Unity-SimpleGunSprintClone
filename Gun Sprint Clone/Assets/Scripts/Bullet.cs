using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
