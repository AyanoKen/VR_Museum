using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door")) 
        {
            LoadLevel2();
        }
    }

    private void LoadLevel2()
    {
        Debug.Log("Ending game");
        Application.Quit();
        
    }
}
