using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveWithMPlatform : MonoBehaviour
{
    private Transform localTrans;

    private void Start(){  localTrans = transform; }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "customMPlatform")
            localTrans.parent = other.transform; 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "customMPlatform")
            localTrans.parent = null;
    } 
}
