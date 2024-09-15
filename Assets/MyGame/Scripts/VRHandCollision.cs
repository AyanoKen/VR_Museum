using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandCollision : MonoBehaviour
{
    public PlayerLevel2 playerScript;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected");
        if (collision.gameObject.CompareTag("TombStone"))
        {
            playerScript.HandleTombstoneCollision(collision.gameObject);
        }
    }
}
