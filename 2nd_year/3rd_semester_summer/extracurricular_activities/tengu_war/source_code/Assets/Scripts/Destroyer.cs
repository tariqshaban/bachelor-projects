using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        Destroy(collision.gameObject,1);
    }
}