using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PrismaTrigger : MonoBehaviour
{
    [SerializeField] private MonoBehaviour targetScript;

    private void Start()
    {
        if (targetScript == null)
        {
            Debug.LogWarning("Target script is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            if (targetScript != null)
            {
                targetScript.enabled = true;  
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (targetScript != null)
            {
                targetScript.enabled = false;  
            }
        }
    }
}
