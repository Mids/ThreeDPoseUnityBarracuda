using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallParts : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
